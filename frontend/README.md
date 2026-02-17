# Geometrix UI — Frontend

React + Vite frontend for the Geometrix geometric pattern generator.

## Tech Stack
- **React 18** with hooks
- **Vite** for bundling
- **Nginx** for static file serving (production)
- **Docker** + **Kubernetes** for deployment

## Development

```bash
npm install
npm run dev
```

The UI connects to the Geometrix API at `https://geometrix.garry-ai.cloud`.

## Build & Deploy

```bash
# Build the Docker image
docker build -t geometrix-ui:latest .

# Import to k3s
k3s ctr images import <(docker save geometrix-ui:latest)

# Apply K8s manifests
kubectl apply -f ../k8s/frontend/

# Restart deployment
kubectl rollout restart deployment/geometrix-ui -n geometrix
```

## Features
- 🎨 Real-time parameter controls (seed, mirror power, grid, colors)
- 🎲 Randomize button with stale-closure-safe `Generate`
- 📥 Download generated images
- 🖼️ Gallery of 12 last generations (expandable cards)
- 📱 Responsive layout (mobile ≤ 900px)
- 🔗 API Docs link to Swagger UI

## K8s Manifests
See `../k8s/frontend/` for Deployment, Service, and Ingress YAMLs.
