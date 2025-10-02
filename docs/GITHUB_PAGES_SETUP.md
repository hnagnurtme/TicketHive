# 📚 GitHub Pages Setup Guide for TicketHive

This guide explains how to set up and maintain the GitHub Pages documentation for TicketHive API.

## 🚀 Quick Setup

### 1. Enable GitHub Pages

1. Go to your repository on GitHub: `https://github.com/hnagnurtme/TicketHive`
2. Click on **Settings** tab
3. Scroll down to **Pages** section in the left sidebar
4. Under **Source**, select:
   - **Deploy from a branch**
   - **Branch**: `master` or `main`
   - **Folder**: `/docs`
5. Click **Save**

### 2. Alternative: Use GitHub Actions (Recommended)

1. In **Settings** → **Pages**
2. Under **Source**, select **GitHub Actions**
3. The workflow in `.github/workflows/pages.yml` will handle automatic deployment

## 📁 Directory Structure

```
docs/
├── .nojekyll                 # Prevents Jekyll from ignoring underscore files
├── CNAME                     # Custom domain configuration (optional)
├── _config.yml              # Jekyll configuration
├── _layouts/
│   └── default.html         # Default page layout
├── index.md                 # Main landing page
├── demo.html                # Quick demo page
├── swagger.json             # OpenAPI specification
└── swagger-ui/              # Swagger UI files
    ├── index.html           # Swagger UI main page
    ├── swagger-ui-bundle.js
    ├── swagger-ui.css
    ├── swagger-ui-standalone-preset.js
    ├── favicon-32x32.png
    ├── favicon-16x16.png
    └── README.md
```

## 🔗 Access URLs

Once GitHub Pages is enabled, your documentation will be available at:

- **Main Page**: `https://hnagnurtme.github.io/TicketHive/`
- **Swagger UI**: `https://hnagnurtme.github.io/TicketHive/swagger-ui/`
- **Demo Page**: `https://hnagnurtme.github.io/TicketHive/demo.html`
- **OpenAPI Spec**: `https://hnagnurtme.github.io/TicketHive/swagger.json`

## 🔄 Updating Documentation

### Update Swagger Specification

1. Update `docs/swagger.json` with your latest API specification
2. Commit and push changes
3. GitHub Pages will automatically update

### Update Swagger UI

Run the update script from the project root:

```bash
./update-swagger-ui.sh
```

This will download the latest Swagger UI files.

### Manual Swagger UI Update

```bash
cd docs/swagger-ui/
curl -o swagger-ui-bundle.js https://unpkg.com/swagger-ui-dist@5.9.0/swagger-ui-bundle.js
curl -o swagger-ui.css https://unpkg.com/swagger-ui-dist@5.9.0/swagger-ui.css
curl -o swagger-ui-standalone-preset.js https://unpkg.com/swagger-ui-dist@5.9.0/swagger-ui-standalone-preset.js
```

## 🎨 Customization

### Modify Landing Page

Edit `docs/index.md` to update the main landing page content.

### Customize Swagger UI

Edit `docs/swagger-ui/index.html` to:
- Change colors and styling
- Add custom headers/footers
- Modify Swagger UI configuration
- Add custom JavaScript functionality

### Update Branding

Modify the CSS in `docs/swagger-ui/index.html` or `docs/_layouts/default.html` to match your brand colors and styling.

## 🔧 Troubleshooting

### Page Not Loading

1. Check if GitHub Pages is enabled in repository settings
2. Ensure the source is set to `/docs` folder
3. Wait 5-10 minutes for initial deployment
4. Check GitHub Actions tab for deployment status

### Swagger UI Not Working

1. Verify `swagger.json` is valid JSON
2. Check browser console for JavaScript errors
3. Ensure all Swagger UI files are present and not corrupted
4. Verify the path to `swagger.json` in `index.html`

### Jekyll Build Errors

1. Check the Pages deployment in GitHub Actions
2. Verify `_config.yml` syntax
3. Ensure all Markdown files have proper frontmatter
4. Check for any forbidden characters in file names

## 📝 Custom Domain (Optional)

To use a custom domain like `api-docs.tickethive.com`:

1. Update `docs/CNAME` file with your domain
2. Configure DNS CNAME record pointing to `hnagnurtme.github.io`
3. Enable HTTPS in GitHub Pages settings

## 🔐 Security Considerations

- Never commit sensitive API keys or credentials
- Use environment variables for sensitive configuration
- Regularly update Swagger UI to latest version for security patches
- Consider enabling security headers in your web server

## 📊 Analytics (Optional)

Add Google Analytics or other tracking to monitor documentation usage:

1. Get tracking ID from your analytics provider
2. Add tracking code to `docs/_layouts/default.html`
3. Update privacy policy if required

## 🤝 Contributing

When contributing to documentation:

1. Test changes locally by opening `docs/index.md` or `docs/swagger-ui/index.html`
2. Ensure all links work correctly
3. Verify responsive design on mobile devices
4. Update this guide if you add new features

## 📞 Support

If you encounter issues with GitHub Pages setup:

1. Check [GitHub Pages documentation](https://docs.github.com/en/pages)
2. Review [Jekyll documentation](https://jekyllrb.com/docs/)
3. Check the repository Issues section
4. Contact the development team

---

Last updated: October 3, 2025