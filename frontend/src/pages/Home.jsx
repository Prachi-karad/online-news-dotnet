import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { articleApi, errorMessage } from '../api'
import ArticleCard from '../components/ArticleCard'
import Pagination from '../components/Pagination'

export default function Home() {
  const { categoryId } = useParams()
  const [featured, setFeatured] = useState([])
  const [trending, setTrending] = useState([])
  const [result, setResult] = useState(null)
  const [search, setSearch] = useState('')
  const [page, setPage] = useState(0)
  const [error, setError] = useState('')

  // Reset page when category or search changes
  useEffect(() => { setPage(0) }, [categoryId, search])

  useEffect(() => {
    articleApi.featured().then(setFeatured).catch(() => {})
    articleApi.trending().then(setTrending).catch(() => {})
  }, [])

  useEffect(() => {
    const timer = setTimeout(() => {
      articleApi
        .search({ categoryId, search: search || undefined, page, size: 6 })
        .then((data) => { setResult(data); setError('') })
        .catch((err) => setError(errorMessage(err)))
    }, 300)
    return () => clearTimeout(timer)
  }, [categoryId, search, page])

  const showHero = !categoryId && !search && page === 0 && featured.length > 0
  const categoryName = categoryId && result?.content[0]?.category?.name

  return (
    <div className="home">
      {showHero && (
        <section className="hero">
          <ArticleCard article={featured[0]} large />
          <div className="hero-side">
            {featured.slice(1, 3).map((a) => <ArticleCard key={a.id} article={a} />)}
          </div>
        </section>
      )}

      <div className="layout">
        <section>
          <div className="section-head">
            <h2>{search ? `Results for "${search}"` : categoryName || 'Latest News'}</h2>
            <input className="search" placeholder="Search news..." value={search}
                   onChange={(e) => setSearch(e.target.value)} />
          </div>
          {error && <p className="error">{error}</p>}
          {!result ? <p className="empty">Loading...</p> : result.content.length === 0 ? (
            <p className="empty">No articles found.</p>
          ) : (
            <>
              <div className="grid">
                {result.content.map((a) => <ArticleCard key={a.id} article={a} />)}
              </div>
              <Pagination page={result.page} totalPages={result.totalPages} onChange={setPage} />
            </>
          )}
        </section>

        <aside className="sidebar">
          <h2>🔥 Trending</h2>
          <ol className="trending">
            {trending.map((a) => (
              <li key={a.id}>
                <Link to={`/articles/${a.id}`}>{a.title}</Link>
                <small className="muted">{a.views} views</small>
              </li>
            ))}
          </ol>
        </aside>
      </div>
    </div>
  )
}
