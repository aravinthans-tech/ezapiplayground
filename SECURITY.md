# Security Guide

## ⚠️ CRITICAL: Removing Exposed Credentials from Git History

If you've accidentally committed sensitive credentials (API keys, passwords, etc.) to Git, follow these steps:

### Step 1: Rotate All Exposed Keys Immediately

**Before doing anything else**, rotate/revoke ALL exposed credentials:
- AWS Access Keys (create new keys in AWS IAM console)
- Google Maps API Key (regenerate in Google Cloud Console)
- OpenRouter API Key (regenerate in OpenRouter dashboard)
- Unstract API Key (regenerate in Unstract dashboard)
- Database passwords (change in your database server)
- Any other exposed secrets

### Step 2: Remove File from Git History

The sensitive data is still in Git history even if you delete the file. You need to remove it from history:

#### Option A: Using git filter-branch (Recommended for small repos)

```bash
# WARNING: This rewrites Git history. Coordinate with your team first!

# Remove appsettings.json from entire Git history
git filter-branch --force --index-filter \
  "git rm --cached --ignore-unmatch appsettings.json" \
  --prune-empty --tag-name-filter cat -- --all

# Force push to remote (WARNING: This will overwrite remote history)
git push origin --force --all
git push origin --force --tags
```

#### Option B: Using BFG Repo-Cleaner (Faster for large repos)

1. Download BFG: https://rtyley.github.io/bfg-repo-cleaner/
2. Run:
```bash
java -jar bfg.jar --delete-files appsettings.json
git reflog expire --expire=now --all
git gc --prune=now --aggressive
git push origin --force --all
```

### Step 3: Update Environment Variables in Production

After removing from Git history, update all environment variables in Render:
- Go to Render dashboard → Your service → Environment
- Update all sensitive environment variables with new rotated keys
- Redeploy the service

### Step 4: Verify .gitignore

Ensure `.gitignore` includes:
```
appsettings.json
appsettings.Development.json
appsettings.*.json
!appsettings.template.json
```

### Step 5: Create Local Configuration

1. Copy the template:
   ```bash
   cp appsettings.template.json appsettings.json
   ```
2. Fill in your local development credentials (these won't be committed)

### Step 6: Notify AWS Support

After completing steps 1-3, respond to the AWS Support case to confirm:
- All exposed keys have been rotated
- The file has been removed from Git history
- Your account is secured

## Best Practices Going Forward

1. **Never commit `appsettings.json`** - It's in `.gitignore` for a reason
2. **Use environment variables** for all production deployments
3. **Use `appsettings.template.json`** as a reference for required configuration
4. **Review commits** before pushing to ensure no secrets are included
5. **Use secret scanning tools** like GitHub's secret scanning or GitGuardian
6. **Rotate keys regularly** as part of your security practice

## Current Status

✅ `appsettings.json` is in `.gitignore`
✅ `appsettings.template.json` created with placeholders
✅ All sensitive values removed from current `appsettings.json`
✅ **COMPLETED**: `appsettings.json` has been removed from entire Git history
✅ **COMPLETED**: Force pushed to GitHub - remote repository cleaned

**Note**: The file has been completely removed from all commits in the repository history. However, you should still:
1. Rotate all exposed AWS keys and other credentials
2. Update environment variables in Render with new keys
3. Respond to AWS Support to confirm account security

