import { useState, useCallback, useRef } from 'react'
import './App.css'

const API_BASE = 'https://geometrix.garry-ai.cloud'

function resolveImageUrl(fileLocation) {
  if (!fileLocation) return ''
  // Force HTTPS to avoid mixed-content blocking when the site is served over TLS.
  // The API may return http:// URLs; we normalise them to https://.
  if (fileLocation.startsWith('http://') || fileLocation.startsWith('https://')) {
    return fileLocation.replace(/^http:\/\//, 'https://')
  }
  return `${API_BASE}${fileLocation}`
}

// These values must match the ThemeColor value object on the API side.
// Any value outside this list will cause a 400 validation error.
const BG_COLORS = [
  { value: 'dark', label: 'Dark', hex: '#111118' },
  { value: 'light', label: 'Light', hex: '#f0f0f8' },
  { value: 'red', label: 'Red', hex: '#ef4444' },
  { value: 'yellow', label: 'Yellow', hex: '#eab308' },
  { value: 'green', label: 'Green', hex: '#22c55e' },
  { value: 'blue', label: 'Blue', hex: '#3b82f6' },
  { value: 'indigo', label: 'Indigo', hex: '#6366f1' },
  { value: 'purple', label: 'Purple', hex: '#a855f7' },
  { value: 'pink', label: 'Pink', hex: '#ec4899' },
]

const FG_COLORS = [
  { value: 'indigo', label: 'Indigo', hex: '#6366f1' },
  { value: 'purple', label: 'Purple', hex: '#a855f7' },
  { value: 'blue', label: 'Blue', hex: '#3b82f6' },
  { value: 'green', label: 'Green', hex: '#22c55e' },
  { value: 'yellow', label: 'Yellow', hex: '#eab308' },
  { value: 'red', label: 'Red', hex: '#ef4444' },
  { value: 'pink', label: 'Pink', hex: '#ec4899' },
  { value: 'light', label: 'Light', hex: '#f0f0f8' },
  { value: 'dark', label: 'Dark', hex: '#111118' },
]

const defaultParams = {
  seed: 42,
  mirrorPowerHorizontal: 2,
  mirrorPowerVertical: 2,
  cellGroupLength: 4,
  cellWidthPixel: 64,
  backgroundColor: 'dark',
  foregroundColor: 'indigo',
  includeEmptyAndFill: true,
}

function randomSeed() {
  return Math.floor(Math.random() * 999999) + 1
}

function SliderControl({ label, value, min, max, step = 1, onChange, description }) {
  return (
    <div className="control-group">
      <div className="control-header">
        <label>{label}</label>
        <span className="control-value">{value}</span>
      </div>
      {description && <p className="control-desc">{description}</p>}
      <input
        type="range"
        min={min}
        max={max}
        step={step}
        value={value}
        onChange={e => onChange(Number(e.target.value))}
        className="slider"
      />
      <div className="slider-ticks">
        <span>{min}</span>
        <span>{max}</span>
      </div>
    </div>
  )
}

function ColorPicker({ label, value, options, onChange }) {
  return (
    <div className="control-group">
      <label>{label}</label>
      <div className="color-grid">
        {options.map(opt => (
          <button
            key={opt.value}
            className={`color-swatch ${value === opt.value ? 'active' : ''}`}
            style={{ '--swatch-color': opt.hex }}
            onClick={() => onChange(opt.value)}
            title={opt.label}
          >
            <span className="swatch-dot" style={{ background: opt.hex }} />
          </button>
        ))}
      </div>
      <div className="color-selected">
        <span className="color-dot" style={{ background: options.find(o => o.value === value)?.hex }} />
        <span>{options.find(o => o.value === value)?.label}</span>
      </div>
    </div>
  )
}

function ToggleControl({ label, value, onChange, description }) {
  return (
    <div className="control-group toggle-group">
      <div className="toggle-row">
        <div>
          <label>{label}</label>
          {description && <p className="control-desc">{description}</p>}
        </div>
        <button
          className={`toggle ${value ? 'on' : 'off'}`}
          onClick={() => onChange(!value)}
          aria-checked={value}
          role="switch"
        >
          <span className="toggle-thumb" />
        </button>
      </div>
    </div>
  )
}

function ImageCard({ item, index }) {
  const [expanded, setExpanded] = useState(false)
  return (
    <div className={`gallery-card ${expanded ? 'expanded' : ''}`} onClick={() => setExpanded(!expanded)}>
      <div className="gallery-img-wrap">
        <img src={resolveImageUrl(item.fileLocation)} alt={`Generated #${index + 1}`} loading="lazy" />
      </div>
      <div className="gallery-meta">
        <span className="gallery-seed">Seed {item.imageModel.seed}</span>
        <span className="gallery-colors">
          <span style={{ color: FG_COLORS.find(c => c.value === item.imageModel.foregroundColor)?.hex || '#fff' }}>
            {item.imageModel.foregroundColor}
          </span>
          {' on '}
          <span>{item.imageModel.backgroundColor}</span>
        </span>
        <span className="gallery-size">{item.imageModel.widthPixel}×{item.imageModel.heightPixel}px</span>
      </div>
    </div>
  )
}

export default function App() {
  const [params, setParams] = useState(defaultParams)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)
  const [result, setResult] = useState(null)
  const [gallery, setGallery] = useState([])
  const resultRef = useRef(null)

  // Always-fresh mirror of params — eliminates stale-closure race conditions.
  // Assigned synchronously in render so any handler calling generate() sees
  // the latest params regardless of pending setState batches.
  const paramsRef = useRef(params)
  paramsRef.current = params

  const setParam = useCallback((key, value) => {
    setParams(p => ({ ...p, [key]: value }))
  }, [])

  // generate reads from paramsRef.current (always fresh) instead of the
  // params closure captured at memoisation time. No [params] dependency
  // means the same stable function reference is used across renders, so
  // clicking "Generate" right after "Randomize" always picks up the new seed.
  const generate = useCallback(async () => {
    const p = paramsRef.current
    // Defensive fallback: if seed is missing or zero, generate one on the
    // spot so the API never receives an empty/invalid seed.
    const seed = (p.seed && Number(p.seed) > 0) ? Number(p.seed) : randomSeed()

    setLoading(true)
    setError(null)

    const formData = new FormData()
    formData.append('seed', seed)
    formData.append('mirrorPowerHorizontal', p.mirrorPowerHorizontal)
    formData.append('mirrorPowerVertical', p.mirrorPowerVertical)
    formData.append('cellGroupLength', p.cellGroupLength)
    formData.append('cellWidthPixel', p.cellWidthPixel)
    formData.append('backgroundColor', p.backgroundColor)
    formData.append('foregroundColor', p.foregroundColor)
    formData.append('includeEmptyAndFill', p.includeEmptyAndFill)

    try {
      const res = await fetch(`${API_BASE}/api/v1/Images`, {
        method: 'POST',
        body: formData,
      })

      if (!res.ok) {
        const text = await res.text()
        throw new Error(`API error ${res.status}: ${text}`)
      }

      const data = await res.json()
      setResult(data)
      setGallery(prev => [data, ...prev].slice(0, 12))

      setTimeout(() => {
        resultRef.current?.scrollIntoView({ behavior: 'smooth', block: 'center' })
      }, 100)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }, []) // stable reference — reads fresh params via paramsRef

  const handleRandomize = useCallback(() => {
    setParams(p => ({
      ...p,
      seed: randomSeed(),
      mirrorPowerHorizontal: Math.floor(Math.random() * 4) + 1,
      mirrorPowerVertical: Math.floor(Math.random() * 4) + 1,
      cellGroupLength: Math.floor(Math.random() * 6) + 2,
      cellWidthPixel: [32, 48, 64, 80, 96][Math.floor(Math.random() * 5)],
      foregroundColor: FG_COLORS[Math.floor(Math.random() * FG_COLORS.length)].value,
    }))
  }, [])

  // "New Seed" handler: patches paramsRef synchronously so generate() reads
  // the new seed immediately, without waiting for the setState re-render.
  const handleNewSeed = useCallback(() => {
    const newSeed = randomSeed()
    paramsRef.current = { ...paramsRef.current, seed: newSeed }
    setParam('seed', newSeed)
    generate()
  }, [generate, setParam])

  const handleDownload = useCallback(() => {
    if (!result) return
    const url = resolveImageUrl(result.fileLocation)
    const a = document.createElement('a')
    a.href = url
    a.download = `geometrix-${result.imageModel.seed}.png`
    // Must be in the DOM for Firefox & Safari to trigger download reliably.
    document.body.appendChild(a)
    a.click()
    document.body.removeChild(a)
  }, [result])

  return (
    <div className="app">
      {/* Header */}
      <header className="header">
        <div className="header-inner">
          <div className="logo">
            <svg width="32" height="32" viewBox="0 0 32 32" fill="none">
              <polygon points="16,2 30,28 2,28" fill="none" stroke="#7c5cfc" strokeWidth="2"/>
              <polygon points="16,8 24,24 8,24" fill="#7c5cfc" opacity="0.3"/>
              <circle cx="16" cy="16" r="4" fill="#7c5cfc"/>
            </svg>
            <div>
              <h1>Geometrix</h1>
              <span>Geometric Pattern Generator</span>
            </div>
          </div>
          <a href="https://geometrix.garry-ai.cloud/swagger/index.html" target="_blank" rel="noopener noreferrer" className="api-link">
            API Docs ↗
          </a>
        </div>
      </header>

      <main className="main">
        <div className="layout">
          {/* Controls Panel */}
          <aside className="controls-panel">
            <div className="panel-header">
              <h2>Parameters</h2>
              <button className="btn-ghost" onClick={handleRandomize} title="Randomize parameters">
                🎲 Randomize
              </button>
            </div>

            <div className="controls-body">
              {/* Seed */}
              <div className="control-group seed-group">
                <div className="control-header">
                  <label>Seed</label>
                  <span className="control-value mono">{params.seed}</span>
                </div>
                <p className="control-desc">Deterministic random source for pattern generation</p>
                <div className="seed-row">
                  <input
                    type="number"
                    value={params.seed}
                    min={1}
                    max={999999}
                    onChange={e => setParam('seed', Number(e.target.value))}
                    className="seed-input"
                  />
                  <button className="btn-icon" onClick={() => setParam('seed', randomSeed())} title="Random seed">
                    🎲
                  </button>
                </div>
              </div>

              <div className="section-title">Mirror</div>

              <SliderControl
                label="Horizontal Mirror Power"
                value={params.mirrorPowerHorizontal}
                min={1}
                max={6}
                onChange={v => setParam('mirrorPowerHorizontal', v)}
                description="Symmetry repetitions on the X axis"
              />
              <SliderControl
                label="Vertical Mirror Power"
                value={params.mirrorPowerVertical}
                min={1}
                max={6}
                onChange={v => setParam('mirrorPowerVertical', v)}
                description="Symmetry repetitions on the Y axis"
              />

              <div className="section-title">Grid</div>

              <SliderControl
                label="Cell Group Length"
                value={params.cellGroupLength}
                min={1}
                max={10}
                onChange={v => setParam('cellGroupLength', v)}
                description="Number of cells per tile group"
              />
              <SliderControl
                label="Cell Width (px)"
                value={params.cellWidthPixel}
                min={16}
                max={128}
                step={8}
                onChange={v => setParam('cellWidthPixel', v)}
                description="Size of each individual cell in pixels"
              />

              <div className="section-title">Colors</div>

              <ColorPicker
                label="Background Color"
                value={params.backgroundColor}
                options={BG_COLORS}
                onChange={v => setParam('backgroundColor', v)}
              />
              <ColorPicker
                label="Foreground Color"
                value={params.foregroundColor}
                options={FG_COLORS}
                onChange={v => setParam('foregroundColor', v)}
              />

              <div className="section-title">Options</div>

              <ToggleControl
                label="Include Empty & Fill"
                value={params.includeEmptyAndFill}
                onChange={v => setParam('includeEmptyAndFill', v)}
                description="Add empty and filled cell types to the palette"
              />
            </div>

            <div className="controls-footer">
              <button
                className={`btn-generate ${loading ? 'loading' : ''}`}
                onClick={generate}
                disabled={loading}
              >
                {loading ? (
                  <>
                    <span className="spinner" /> Generating…
                  </>
                ) : (
                  <>
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <polygon points="5 3 19 12 5 21 5 3"/>
                    </svg>
                    Generate Pattern
                  </>
                )}
              </button>
            </div>
          </aside>

          {/* Result Panel */}
          <section className="result-panel">
            {/* Current Result */}
            <div className="result-main" ref={resultRef}>
              {!result && !loading && !error && (
                <div className="empty-state">
                  <div className="empty-icon">
                    <svg width="64" height="64" viewBox="0 0 64 64" fill="none">
                      <polygon points="32,4 60,56 4,56" fill="none" stroke="#3a3a4a" strokeWidth="2"/>
                      <polygon points="32,16 48,48 16,48" fill="#2a2a3a" opacity="0.6"/>
                      <circle cx="32" cy="32" r="6" fill="#3a3a4a"/>
                    </svg>
                  </div>
                  <h3>No pattern yet</h3>
                  <p>Configure the parameters and click <strong>Generate Pattern</strong> to create your first geometric artwork.</p>
                  <button className="btn-generate" onClick={generate}>
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <polygon points="5 3 19 12 5 21 5 3"/>
                    </svg>
                    Generate Pattern
                  </button>
                </div>
              )}

              {loading && (
                <div className="loading-state">
                  <div className="loading-anim">
                    <div className="loading-ring" />
                    <div className="loading-ring delay-1" />
                    <div className="loading-ring delay-2" />
                  </div>
                  <p>Generating your pattern…</p>
                </div>
              )}

              {error && !loading && (
                <div className="error-state">
                  <div className="error-icon">⚠️</div>
                  <h3>Generation failed</h3>
                  <p className="error-msg">{error}</p>
                  <button className="btn-generate" onClick={generate}>Retry</button>
                </div>
              )}

              {result && !loading && (
                <div className="result-content">
                  <div className="result-image-wrap">
                    <img
                      src={resolveImageUrl(result.fileLocation)}
                      alt="Generated geometric pattern"
                      className="result-image"
                    />
                  </div>
                  <div className="result-info">
                    <div className="result-meta">
                      <div className="meta-grid">
                        <div className="meta-item">
                          <span className="meta-label">Seed</span>
                          <span className="meta-value mono">{result.imageModel.seed}</span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Size</span>
                          <span className="meta-value mono">{result.imageModel.widthPixel} × {result.imageModel.heightPixel}px</span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Grid</span>
                          <span className="meta-value mono">{result.imageModel.horizontalCell} × {result.imageModel.verticalCell} cells</span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Cell Size</span>
                          <span className="meta-value mono">{result.imageModel.cellWidthPixel}px</span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Mirror H</span>
                          <span className="meta-value mono">{result.imageModel.mirrorPowerHorizontal}×</span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Mirror V</span>
                          <span className="meta-value mono">{result.imageModel.mirrorPowerVertical}×</span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Background</span>
                          <span className="meta-value">
                            <span className="color-dot-sm" style={{ background: BG_COLORS.find(c => c.value === result.imageModel.backgroundColor)?.hex || '#666' }} />
                            {result.imageModel.backgroundColor}
                          </span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Foreground</span>
                          <span className="meta-value">
                            <span className="color-dot-sm" style={{ background: FG_COLORS.find(c => c.value === result.imageModel.foregroundColor)?.hex || '#666' }} />
                            {result.imageModel.foregroundColor}
                          </span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">Cell Group</span>
                          <span className="meta-value mono">{result.imageModel.cellGroupLength}</span>
                        </div>
                        <div className="meta-item">
                          <span className="meta-label">ID</span>
                          <span className="meta-value mono small">{result.imageModel.id}</span>
                        </div>
                      </div>
                    </div>
                    <div className="result-actions">
                      <button className="btn-secondary" onClick={handleDownload}>
                        ↓ Download
                      </button>
                      <button className="btn-secondary" onClick={() => window.open(resolveImageUrl(result.fileLocation), '_blank')}>
                        ↗ Open Full
                      </button>
                      <button className="btn-secondary" onClick={handleNewSeed}>
                        🎲 New Seed
                      </button>
                    </div>
                  </div>
                </div>
              )}
            </div>

            {/* Gallery */}
            {gallery.length > 0 && (
              <div className="gallery-section">
                <h3 className="gallery-title">
                  Recent Generations
                  <span className="gallery-count">{gallery.length}</span>
                </h3>
                <div className="gallery-grid">
                  {gallery.map((item, i) => (
                    <ImageCard key={item.imageModel.id} item={item} index={i} />
                  ))}
                </div>
              </div>
            )}
          </section>
        </div>
      </main>

      <footer className="footer">
        <p>Geometrix — Clean Architecture & DDD · <a href="mailto:philippe@atypical.consulting">Atypical Consulting</a></p>
      </footer>
    </div>
  )
}
