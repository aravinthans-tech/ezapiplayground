# API Playground - Backend & Frontend

A full-stack application with a .NET 8.0 Web API backend and a React frontend for API testing and management.

## Architecture

- **Backend**: .NET 8.0 Web API (API-only, no static files)
- **Frontend**: React application (Create React App) that consumes the backend API

## Features

- **QR Code Generation**: Generate QR codes from text input
- **File Summary**: AI-powered file summarization
- **KYC Agent**: Document verification and KYC processing
- **API Key Management**: Generate and manage API keys
- **Interactive Playground**: Test all APIs directly in the browser
- **Code Examples**: View code examples for all endpoints

## Prerequisites

### Backend
- .NET 8 SDK

### Frontend
- Node.js (v14 or higher)
- npm or yarn

## Setup Instructions

### Backend Setup

1. **Navigate to the project root**
   ```bash
   cd <project-root>
   ```

2. **Configure API Keys and Database**
   
   **IMPORTANT**: Never commit `appsettings.json` with real credentials!
   
   Copy the template and fill in your local development values:
   ```bash
   cp appsettings.template.json appsettings.json
   ```
   
   Then edit `appsettings.json` with your local development credentials:
   ```json
   {
     "ApiKeys": {
       "ValidKeys": [
         "your-api-key-here",
         "another-key"
       ]
     },
     "ConnectionStrings": {
       "eZApiTenantContext": "your-connection-string"
     }
   }
   ```
   
   Note: `appsettings.json` is in `.gitignore` and will NOT be committed to Git.

3. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

4. **Run the Backend**
   ```bash
   dotnet run
   ```
   
   The API will be available at:
   - `https://localhost:51347` (HTTPS)
   - `http://localhost:51348` (HTTP)

### Frontend Setup

1. **Navigate to the frontend directory**
   ```bash
   cd frontend
   ```

2. **Install Dependencies**
   ```bash
   npm install
   ```

3. **Configure API Base URL**
   
   Edit `frontend/.env`:
   ```
   REACT_APP_API_BASE_URL=http://localhost:51348
   ```
   
   Update this to match your backend URL and port.

4. **Run the Frontend**
   ```bash
   npm start
   ```
   
   The frontend will be available at `http://localhost:3000`

## Running Both Applications

### Development

1. **Terminal 1 - Backend:**
   ```bash
   cd <project-root>
   dotnet run
   ```

2. **Terminal 2 - Frontend:**
   ```bash
   cd frontend
   npm start
   ```

3. **Access the Application:**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:51348

## API Endpoints

### Generate API Key
- **POST** `/api/Client/apiKey?userName={email}&password={password}`
- **GET** `/api/Client/apiKey?userName={email}&password={password}`

### QR Code Generation
- **POST** `/api/qrcode/generate`
- **Headers**: `X-API-Key: your-api-key`
- **Body**: `{ "qrvalue": "string" }`

### File Summary
- **POST** `/api/filesummary/getSummary`
- **Headers**: `X-API-Key: your-api-key`
- **Body**: `multipart/form-data` with `file` and `token`

### KYC Verification
- **POST** `/api/kycagent/verify`
- **Headers**: `X-API-Key: your-api-key`
- **Body**: `multipart/form-data` with `documents`, `expectedAddress`, `modelChoice`, `consistencyThreshold`, optional `licenseImage`, `selfieImage`

## Project Structure

```
.
├── Controllers/              # API Controllers
│   ├── ApiKeyController.cs
│   ├── QrCodeController.cs
│   ├── FileSummaryController.cs
│   └── KycAgentController.cs
├── Services/                 # Business Logic
├── Middleware/               # API Key Middleware
├── Models/                   # Data Models
├── Program.cs               # Backend startup
├── appsettings.json         # Backend configuration
├── frontend/                # React Frontend
│   ├── src/
│   │   ├── components/      # React components
│   │   ├── services/        # API service layer
│   │   └── App.js          # Main app with routing
│   ├── public/             # Static assets
│   ├── .env                # Frontend configuration
│   └── package.json        # Frontend dependencies
└── wwwroot/                # Archived (original HTML files)
```

## ⚠️ Security Warning

**NEVER commit sensitive credentials to Git!**

This repository has been configured to exclude sensitive files:
- `appsettings.json` - Contains API keys, connection strings, and secrets
- `appsettings.Development.json` - Development-specific secrets

### For Local Development:
1. Copy `appsettings.template.json` to `appsettings.json`
2. Fill in your actual credentials (these will NOT be committed to Git)
3. Never commit `appsettings.json` or `appsettings.Development.json`

### For Production (Render):
All sensitive values must be set as **Environment Variables** in the Render dashboard. See the deployment section below.

### If Credentials Were Exposed:
If you've accidentally committed sensitive data:
1. **Immediately rotate/revoke all exposed keys** (AWS, Google Maps, OpenRouter, etc.)
2. Remove the file from Git history (see below)
3. Update all environment variables in production
4. Never commit sensitive data again

## Configuration

### Backend CORS

The backend is configured to allow requests from:
- `http://localhost:3000` (React dev server)
- `http://localhost:3001` (Alternative React port)

To add more origins, edit `Program.cs`:
```csharp
policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "your-origin")
```

### Frontend API URL

Update `frontend/.env` to point to your backend:
```
REACT_APP_API_BASE_URL=http://localhost:51348
```

## Authentication

All API endpoints (except API key generation) require an `X-API-Key` header:

```
X-API-Key: your-api-key-here
```

Generate an API key using the API Key page in the frontend or by calling:
```
POST /api/Client/apiKey?userName={email}&password={password}
```

## Dependencies

### Backend
- **System.Drawing.Common** (8.0.0) - Image processing
- **Microsoft.Data.SqlClient** (5.1.1) - Database access
- **Newtonsoft.Json** (13.0.3) - JSON processing
- **AWSSDK.Rekognition** (3.7.400.0) - AWS Rekognition for face matching

### Frontend
- **react** - React library
- **react-router-dom** - Routing
- **axios** - HTTP client
- **tailwindcss** - Styling

## Troubleshooting

### CORS Errors

If you see CORS errors, ensure:
1. Backend CORS is configured for your frontend URL
2. Frontend `.env` has the correct backend URL
3. Both applications are running

### API Key Not Working

1. Verify the API key is included in the `X-API-Key` header
2. Check that the API key exists in the database (via API key generation endpoint)
3. Ensure the middleware is properly configured

### Frontend Can't Connect to Backend

1. Verify backend is running and accessible
2. Check `frontend/.env` has correct `REACT_APP_API_BASE_URL`
3. Ensure CORS is properly configured in backend
4. Check browser console for detailed error messages

## Deployment

### Backend Deployment (Render)

1. **GitHub Repository**: `aravinthans-tech/ezapiplayground`
2. **Go to Render Dashboard**: https://render.com
3. **Create New Web Service**:
   - Connect GitHub repository: `aravinthans-tech/ezapiplayground`
   - Name: `ezapiplayground`
   - Environment: **Docker**
   - Build Command: (auto-detected from Dockerfile)
   - Start Command: `dotnet QRCodeAPI.dll`
4. **Set Environment Variables** in Render dashboard:
   - `ASPNETCORE_ENVIRONMENT=Production`
   - `ASPNETCORE_URLS=http://+:8080`
   - `ConnectionStrings__eZApiTenantContext=<your-connection-string>`
   - `ExternalApis__GoogleMaps__ApiKey=<your-google-maps-key>`
   - `ExternalApis__OpenRouter__ApiKey=<your-openrouter-key>`
   - `ExternalApis__Unstract__ApiKey=<your-unstract-key>`
   - `ExternalApis__AwsRekognition__AccessKey=<your-aws-access-key>`
   - `ExternalApis__AwsRekognition__SecretKey=<your-aws-secret-key>`
   - `ExternalApis__AwsRekognition__Region=ap-south-1`
   - `ApiKeys__ValidKeys__0=<your-api-key-1>`
   - `ApiKeys__ValidKeys__1=<your-api-key-2>`
5. **Deploy** and note your backend URL (e.g., `https://ezapiplayground.onrender.com`)

### Frontend Deployment (Vercel)

1. **GitHub Repository**: `aravinthans-tech/ezplaygroundapp`
2. **Go to Vercel Dashboard**: https://vercel.com
3. **Import Project**:
   - Connect GitHub repository: `aravinthans-tech/ezplaygroundapp`
   - Framework Preset: **Create React App**
   - Root Directory: `.` (root of repository)
   - Build Command: `npm run build`
   - Output Directory: `build`
4. **Set Environment Variable**:
   - `REACT_APP_API_BASE_URL`: Your Render backend URL (from step above)
5. **Deploy** and note your frontend URL (e.g., `https://ezplaygroundapp.vercel.app`)

### Post-Deployment

1. **Update Backend CORS**: After getting your Vercel URL, update `Program.cs` CORS policy to include your actual Vercel URL
2. **Commit and Push**: Push the CORS update to trigger Render auto-deployment
3. **Test**: Verify the frontend can communicate with the backend

## Development Notes

- The backend runs independently as a pure API
- The frontend runs independently and makes HTTP requests to the backend
- Both can be deployed separately
- The `wwwroot` directory contains the original HTML files (archived for reference)
- **Important**: `appsettings.json` is excluded from git for security. Use environment variables in production.
