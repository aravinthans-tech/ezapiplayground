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
   
   Edit `appsettings.json` or `appsettings.Development.json`:
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

## Development Notes

- The backend runs independently as a pure API
- The frontend runs independently and makes HTTP requests to the backend
- Both can be deployed separately
- The `wwwroot` directory contains the original HTML files (archived for reference)
