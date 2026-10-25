import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { articleApi, errorMessage } from '../api'
import { formatDate, readingTime, timeAgo } from '../utils'

export default function ArticlePage() {
  const { id } = useParams()
  const [article, setArticle] = useState(null)
  const [comments, setComments] = useState([])
  const [form, setForm] = useState({ name: '', text: '' })
  const [error, setError] = useState('')
  const [commentError, setCommentError] = useState('')

  useEffect(() => {
    window.scrollTo(0, 0)
    setArticle(null)
    articleApi.get(id).then(setArticle).catch((err) => setError(errorMessage(err)))
    articleApi.comments(id).then(setComments).catch(() => setComments([]))
  }, [id])

  const submitComment = async (e) => {
    e.preventDefault()
    try {
      const c = await articleApi.addComment(id, form)
      setComments([c, ...comments])
      setForm({ ...form, text: '' })
      setCommentError('')
    } catch (err) {
      setCommentError(errorMessage(err))
    }
  }

  const share = () => {
    navigator.clipboard?.writeText(window.location.href)
    alert('Link copied to clipboard')
  }

  if (error) return <p className="error">{error}</p>
  if (!article) return <p className="empty">Loading...</p>

  return (
    <article className="article">
      <Link to={`/category/${article.category.id}`} className="tag">{article.category.name}</Link>
      <h1>{article.title}</h1>
      {article.summary && <p className="lead">{article.summary}</p>}
      <div className="meta muted">
        By <strong>{article.author}</strong> · {formatDate(article.publishedAt)} · {readingTime(article.content)} min read · {article.views} views
        <button className="link-btn" onClick={share}>Share 🔗</button>
      </div>
      {article.imageUrl && <img className="article-img" src={article.imageUrl} alt="" />}
      <div className="content">
        {article.content.split(/\n\s*\n/).map((p, i) => <p key={i}>{p}</p>)}
      </div>

      <section className="comments">
        <h2>Comments ({comments.length})</h2>
        <form className="comment-form" onSubmit={submitComment}>
          <input required placeholder="Your name" value={form.name}
                 onChange={(e) => setForm({ ...form, name: e.target.value })} />
          <textarea required placeholder="Write a comment..." value={form.text}
                    onChange={(e) => setForm({ ...form, text: e.target.value })} />
          {commentError && <p className="error">{commentError}</p>}
          <button className="btn">Post comment</button>
        </form>
        {comments.map((c) => (
          <div key={c.id} className="comment">
            <strong>{c.name}</strong> <small className="muted">{timeAgo(c.createdAt)}</small>
            <p>{c.text}</p>
          </div>
        ))}
      </section>
    </article>
  )
}
