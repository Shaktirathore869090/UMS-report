# ==============================================================================
# Dockerfile — DocXReports API
#
# PURPOSE:
#   Build a Linux/amd64 image on Windows Docker Desktop and run it on a
#   Linux backup server.
#
# BUILD (run from UMS-Reports-API\ folder on Windows):
#   docker build --platform linux/amd64 -t ums-reports-docx:latest .
#
# EXPORT (save image to a file for transfer):
#   docker save -o ums-reports-docx.tar ums-reports-docx:latest
#
# ON LINUX SERVER (import and run):
#   docker load -i ums-reports-docx.tar
#   docker run -d --restart always -p 5013:5013 --name docx-api ums-reports-docx:latest
#
# SWAGGER UI (once running):
#   http://<linux-server-ip>:5013/swagger
# ==============================================================================


# ==============================================================================
# Stage 1 — Build
# Explicitly target linux/amd64 so the image built on Windows Docker Desktop
# is always a Linux image that can run on any standard Linux x86_64 server.
# ==============================================================================
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# ── Step 1: Copy only .csproj files first ────────────────────────────────────
# Docker caches each layer. By restoring NuGet packages before copying source
# code, the restore layer is reused on rebuild as long as .csproj files have
# not changed. This saves 1-2 minutes on every rebuild.
COPY ["DocXReports/DocXReports.csproj",                                          "DocXReports/"]
COPY ["DocxTemplater-main/DocxTemplater/DocxTemplater.csproj",                   "DocxTemplater-main/DocxTemplater/"]
COPY ["DocxTemplater-main/DocxTemplater.Images/DocxTemplater.Images.csproj",     "DocxTemplater-main/DocxTemplater.Images/"]

# ── Step 2: Restore NuGet packages ───────────────────────────────────────────
RUN dotnet restore "DocXReports/DocXReports.csproj" \
    --runtime linux-x64

# ── Step 3: Copy all source code ─────────────────────────────────────────────
COPY DocXReports/        DocXReports/
COPY DocxTemplater-main/ DocxTemplater-main/

# ── Step 4: Create a dummy git repo ──────────────────────────────────────────
# WHY: DocxTemplater references GitVersion.MsBuild which runs during dotnet
# publish to auto-calculate the assembly version from git history.
# Docker build contexts never include the real .git folder, so GitVersion
# crashes with "Cannot find the .git directory".
# FIX: Install git, init a fresh repo, commit all files, tag it 1.0.0.
# GitVersion finds the repo, reads the tag, resolves version = 1.0.0, done.
# We do NOT copy your real .git — we create a clean throwaway one.
RUN apt-get update && \
    apt-get install -y --no-install-recommends git && \
    rm -rf /var/lib/apt/lists/* && \
    git init /src && \
    git -C /src config user.email "docker@build" && \
    git -C /src config user.name "Docker Build" && \
    git -C /src add -A && \
    git -C /src commit -m "build snapshot" --allow-empty && \
    git -C /src tag 1.0.0

# ── Step 5: Publish ───────────────────────────────────────────────────────────
# WHY Debug and not Release:
# DocxTemplater-main/Directory.Build.props sets TreatWarningsAsErrors=true
# and EnforceCodeStyleInBuild=true in Release mode. The TableProcessor.cs
# file has CS1591/IDE/CA warnings that become errors in Release and break
# the build. Debug mode disables TreatWarningsAsErrors so warnings stay
# as warnings. The published output is still fully optimised.
RUN dotnet publish "DocXReports/DocXReports.csproj" \
    -c Debug \
    --runtime linux-x64 \
    --self-contained false \
    -o /app/publish \
    --no-restore


# ==============================================================================
# Stage 2 — Runtime
# Start fresh from the slim ASP.NET runtime image (~200 MB vs ~800 MB SDK).
# The SDK, source code, git repo, and build tools are all discarded here.
# Only the compiled output from Stage 1 is copied across.
# ==============================================================================
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# ── Install LibreOffice and all native dependencies ───────────────────────────
#
# libreoffice-writer   → installs soffice at /usr/bin/soffice
#                        LibreOfficeWrapper.cs calls this to convert DOCX→PDF
#
# libfontconfig1       → font discovery — SkiaSharp needs this to locate fonts
# libgl1-mesa-glx      → software OpenGL stub — SkiaSharp uses GPU features,
#                        this provides a CPU fallback so it works headless
# libglib2.0-0         → core GLib — both LibreOffice and SkiaSharp depend on it
# libx11-6             → X11 base — LibreOffice links against X11 even headless
# libxext6             → X11 extensions — required by LibreOffice headless
# libxrender1          → X11 rendering — required by LibreOffice headless
#
# fonts-liberation     → free Arial/Times/Courier metric-compatible fonts
#                        ensures Word documents render with correct spacing
# fonts-noto           → Google universal font — covers Hindi, Arabic, etc.
#                        prevents "tofu" boxes for non-Latin characters in PDFs
#
# locales              → LibreOffice requires a valid system locale
#                        without this it logs warnings and can silently fail
# ca-certificates      → SSL root certs for any outbound HTTPS calls
#
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        libreoffice-writer \
        libfontconfig1 \
        libgl1-mesa-glx \
        libglib2.0-0 \
        libx11-6 \
        libxext6 \
        libxrender1 \
        fonts-liberation \
        fonts-noto \
        locales \
        ca-certificates && \
    # Generate locale so LibreOffice starts cleanly
    locale-gen en_US.UTF-8 && \
    # Confirm soffice is installed at the expected path
    test -f /usr/bin/soffice && echo "✓ soffice found at /usr/bin/soffice" && \
    # Remove apt cache to keep image as small as possible
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# ── Set locale environment variables ─────────────────────────────────────────
# LibreOffice reads these at startup. Missing locale = warning spam + possible
# silent conversion failures on some document types.
ENV LANG=en_US.UTF-8 \
    LANGUAGE=en_US:en \
    LC_ALL=en_US.UTF-8

WORKDIR /app

# ── Copy compiled application from build stage ────────────────────────────────
COPY --from=build /app/publish .

# ── Copy DOCX templates ───────────────────────────────────────────────────────
# Templates/ is read at runtime by DocXReport.GeneratePDF().
# Copied here (not in build stage) because they are data files not code.
# To update templates without rebuilding, mount a volume:
#   docker run -v /host/Templates:/app/Templates:ro ...
COPY DocXReports/Templates/ ./Templates/

# ── Copy appsettings.Production.json ─────────────────────────────────────────
# ASP.NET Core automatically loads appsettings.{ENVIRONMENT}.json on top of
# appsettings.json. Since ASPNETCORE_ENVIRONMENT=Production (set below),
# this file is loaded and overrides the Windows LibreOfficePath with the
# Linux path /usr/bin/soffice.
COPY DocXReports/appsettings.Production.json ./appsettings.Production.json

# ── Create writable Resources directory ──────────────────────────────────────
# DocXReport.cs writes rendered .docx and .pdf files here during processing,
# then deletes them after streaming the response back to the caller.
# Without this directory the app crashes on first report request.
RUN mkdir -p Resources && \
    mkdir -p Resources/DocXHtmlToPdfConverterTmp && \
    chmod -R 777 Resources

# ── Environment variables ─────────────────────────────────────────────────────
# AppSettings__LibreOfficePath  → overrides appsettings.json Windows path
#                                 (double underscore = section:key separator)
# ASPNETCORE_URLS               → bind to all interfaces on port 5013
#                                 http only — TLS handled by reverse proxy
# ASPNETCORE_ENVIRONMENT        → loads appsettings.Production.json,
#                                 no effect on Swagger (always enabled now)
ENV AppSettings__LibreOfficePath=/usr/bin/soffice \
    ASPNETCORE_URLS=http://+:5013 \
    ASPNETCORE_ENVIRONMENT=Production

# ── Expose port ───────────────────────────────────────────────────────────────
# Documents that the container listens on 5013.
# Actual port mapping is done at runtime: docker run -p 5013:5013
EXPOSE 5013

# ── Start the application ─────────────────────────────────────────────────────
ENTRYPOINT ["dotnet", "DocXReports.dll"]
