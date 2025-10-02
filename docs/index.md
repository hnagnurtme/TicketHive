---
layout: default
title: TicketHive API Documentation
description: Comprehensive API documentation for TicketHive event ticket management platform
---

<div class="container">
    <div class="header">
        <h1>🎫 TicketHive API</h1>
        <p>Comprehensive API Documentation for Event Ticket Management Platform</p>
    </div>
    
    <div class="nav">
        <a href="swagger-ui/" class="primary">📖 View API Documentation</a>
        <a href="https://github.com/hnagnurtme/TicketHive">📦 GitHub Repository</a>
        <a href="https://github.com/hnagnurtme/TicketHive/releases">📥 Download</a>
    </div>
    
    <div class="features">
        <div class="feature">
            <h3>🔐 Authentication & Security</h3>
            <p>JWT-based authentication with email verification, refresh tokens, and secure password handling for robust security.</p>
        </div>
        
        <div class="feature">
            <h3>🎪 Event Management</h3>
            <p>Complete event lifecycle management including creation, publishing, status tracking, and advanced filtering capabilities.</p>
        </div>
        
        <div class="feature">
            <h3>🎟️ Ticket Operations</h3>
            <p>Comprehensive ticket management with activation controls, event-specific retrieval, and powerful sorting options.</p>
        </div>
        
        <div class="feature">
            <h3>👤 User Profiles</h3>
            <p>Secure user registration and profile management with email verification and profile update capabilities.</p>
        </div>
        
        <div class="feature">
            <h3>🛠️ Developer Friendly</h3>
            <p>Built with .NET 8.0, Clean Architecture, comprehensive testing, and extensive API documentation.</p>
        </div>
        
        <div class="feature">
            <h3>🐳 Production Ready</h3>
            <p>Docker support, Entity Framework Core, advanced logging, and deployment-ready configuration.</p>
        </div>
    </div>
    
    <div class="content">
        <h2>🚀 Quick Start</h2>
        <p>Get started with the TicketHive API in minutes:</p>
        
        <h3>1. Authentication</h3>
        <p>Register and authenticate to get your JWT token:</p>
        <pre><code>POST /api/auth/register
POST /api/auth/login</code></pre>
        
        <h3>2. Create Events</h3>
        <p>Create and manage your events:</p>
        <pre><code>POST /api/events
GET /api/events
PUT /api/events/{id}</code></pre>
        
        <h3>3. Manage Tickets</h3>
        <p>Handle ticket operations with full control:</p>
        <pre><code>POST /api/tickets
GET /api/tickets
GET /api/tickets/events/{eventId}</code></pre>
        
        <h2>📋 Core Features</h2>
        <ul>
            <li><strong>RESTful API</strong> - Clean, predictable URLs and HTTP methods</li>
            <li><strong>JSON Responses</strong> - Consistent data format across all endpoints</li>
            <li><strong>JWT Authentication</strong> - Secure token-based authentication</li>
            <li><strong>Pagination</strong> - Efficient data retrieval for large datasets</li>
            <li><strong>Filtering & Sorting</strong> - Flexible query capabilities</li>
            <li><strong>Error Handling</strong> - Comprehensive error responses with details</li>
            <li><strong>Rate Limiting</strong> - Protection against abuse and overuse</li>
            <li><strong>CORS Support</strong> - Cross-origin resource sharing enabled</li>
        </ul>
        
        <h2>🔗 Base URL</h2>
        <p>All API requests should be made to:</p>
        <pre><code>https://api.tickethive.com/</code></pre>
        
        <h2>📊 Status Codes</h2>
        <p>The API uses standard HTTP status codes:</p>
        <ul>
            <li><code>200</code> - OK: Request successful</li>
            <li><code>201</code> - Created: Resource created successfully</li>
            <li><code>400</code> - Bad Request: Invalid request parameters</li>
            <li><code>401</code> - Unauthorized: Authentication required</li>
            <li><code>403</code> - Forbidden: Insufficient permissions</li>
            <li><code>404</code> - Not Found: Resource not found</li>
            <li><code>500</code> - Internal Server Error: Server error</li>
        </ul>
    </div>
    
    <div class="footer">
        <p>Made with ❤️ by the TicketHive Team | 
        <a href="https://github.com/hnagnurtme/TicketHive">GitHub</a> | 
        <a href="https://opensource.org/licenses/MIT">MIT License</a></p>
    </div>
</div>