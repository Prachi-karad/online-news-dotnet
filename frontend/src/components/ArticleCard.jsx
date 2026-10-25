import { Link } from 'react-router-dom'
import { timeAgo } from '../utils'

export default function ArticleCard({ article, large = false }) {
  return (
    <Link to={`/articles/${article.id}`} className={`article-card ${large ? 'large' : ''}`}>
      {article.imageUrl && <img src={article.imageUrl} alt="" loading="lazy" />}
      <div className="article-card-body">
        <span className="tag">{article.category?.name}</span>
        <h3>{article.title}</h3>
        {article.summary && <p>{article.summary}</p>}
        <small className="muted">{article.author} · {timeAgo(article.publishedAt)}</small>
      </div>
    </Link>
  )
}
