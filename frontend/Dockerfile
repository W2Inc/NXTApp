# Install dependencies only when needed
FROM oven/bun:canary-alpine AS deps
WORKDIR /app

# Copy package files and prisma directory
COPY package.json bun.lockb* yarn.lock* package-lock.json* ./

# Install dependencies
RUN bun install

# Build stage
FROM oven/bun:canary-alpine AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .

# Build the application
RUN bun run build

# Production stage
FROM oven/bun:canary-alpine AS runner
WORKDIR /app

ENV NODE_ENV=production

# Create a non-root user
RUN addgroup --system --gid 1001 bunjs
RUN adduser --system --uid 1001 bunjs

# Copy package.json and build output
COPY --from=builder /app/package.json ./package.json
COPY --from=builder --chown=bunjs:bunjs /app/build ./build

USER bunjs

EXPOSE 3000
ENV PORT=3000

# Start the application with Bun
CMD ["bun", "--bun", "run", "/app/build"]
