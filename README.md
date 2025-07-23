# E-Commerce Solution (ASP.NET Core + Vue.js)

A complete e-commerce solution with a .NET Core backend and Vue.js frontend, featuring product listings, search, filtering, pagination, and product details.

## Features

### Backend (ASP.NET Core)
- RESTful API endpoints
- Product search with filtering (category, price range)
- Pagination support
- Related products functionality
- CORS configuration for frontend access
- Swagger API documentation

### Frontend (Vue.js)
- Responsive product grid
- Advanced filtering and search
- Pagination with page size options
- Product details page
- Related products section
- Toast notifications
- Error handling

## Technologies Used

- **Backend**: ASP.NET Core 7, C#
- **Frontend**: Vue.js 3, Vue Router
- **Styling**: CSS Grid, Flexbox, custom animations
- **Build Tools**: Vite

## Getting Started

### Prerequisites
- .NET 7 SDK
- Node.js (v16+ recommended)
- npm or yarn

### Backend Setup
1. Navigate to the backend directory
2. Run `dotnet restore` to restore packages
3. Run `dotnet run` to start the server
4. The API will be available at `https://localhost:7240`
5. Swagger UI will be available at `https://localhost:7240/swagger`

### Frontend Setup
1. Navigate to the frontend directory
2. Run `npm install` to install dependencies
3. Run `npm run dev` to start the development server
4. The app will be available at `http://localhost:5173`

## Project Structure

```
ecommerce-solution/
│
├── backend/                  # ASP.NET Core project
│   ├── Controllers/          # API controllers
│   ├── Models/               # Data models
│   ├── Services/             # Business logic
│   ├── Program.cs            # Application entry point
│   └── appsettings.json      # Configuration
│
├── frontend/                 # Vue.js project
│   ├── public/               # Static files
│   ├── src/
│   │   ├── assets/           # Static assets
│   │   ├── components/       # Vue components
│   │   ├── views/            # Vue views
│   │   ├── App.vue           # Main app component
│   │   └── main.js           # App entry point
│   ├── package.json          # NPM dependencies
│   └── vite.config.js        # Vite configuration
│
└── README.md                 # Project documentation
```

## API Endpoints

- `GET /api/products` - Get products with optional filtering
- `GET /api/products/{id}` - Get single product details
- `GET /api/products/{id}/related` - Get related products
- `GET /api/products/categories` - Get all categories

## Future Improvements

- Add user authentication
- Implement shopping cart functionality
- Add product reviews and ratings
- Implement checkout process<img width="1815" height="969" alt="Screenshot 2025-07-23 133451" src="https://github.com/user-attachments/assets/ace26d08-0202-4742-9d97-a0193364b16f" />
<img width="1819" height="967" alt="Screenshot 2025-07-23 133508" src="https://github.com/user-attachments/assets/dadb0bc2-7f23-438f-8e78-033f9de6ed35" />
<img width="1790" height="985" alt="Screenshot 2025-07-23 133522" src="https://github.com/user-attachments/assets/cb7755fe-0ee3-4d6d-bf9a-281c9c06ab7e" />

- Add admin dashboard for product management
