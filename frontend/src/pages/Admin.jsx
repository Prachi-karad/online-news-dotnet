import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { articleApi, categoryApi, errorMessage } from '../api'
import { formatDate } from '../utils'
import Pagination from '../components/Pagination'

const emptyArticle = { title: '', summary: '', content: '', imageUrl: '', author: '', categoryId: '', featured: false }

export default function Admin() {
  const [categories, setCategories] = useState([])
  const [result, setResult] = useState(null)
  const [page, setPage] = useState(0)
  const [form, setForm] = useState(emptyArticle)
  const [editingId, setEditingId] = useState(null)
  const [newCategory, setNewCategory] = useState('')
  const [msg, setMsg] = useState('')
  const [error, setError] = useState('')

  const loadArticles = () => articleApi.search({ page, size: 10 }).then(setResult)
  const loadCategories = () => categoryApi.list().then(setCategories)

  useEffect(() => { loadCategories() }, [])
  useEffect(() => { loadArticles() }, [page])

  const run = async (fn, success) => {
    setMsg(''); setError('')
    try { await fn(); setMsg(success) } catch (err) { setError(errorMessage(err)) }
  }

  const save = (e) => {
    e.preventDefault()
    const payload = { ...form, categoryId: Number(form.categoryId) }
    run(async () => {
      if (editingId) await articleApi.update(editingId, payload)
      else await articleApi.create(payload)
      setForm(emptyArticle); setEditingId(null); loadArticles()
    }, editingId ? 'Article updated' : 'Article published')
  }

  const edit = async (id) => {
    const a = await articleApi.get(id, false)
    setForm({
      title: a.title, summary: a.summary || '', content: a.content, imageUrl: a.imageUrl || '',
      author: a.author, categoryId: String(a.category.id), featured: a.featured,
    })
    setEditingId(id)
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  const remove = (a) => {
    if (!window.confirm(`Delete "${a.title}"?`)) return
    run(async () => { await articleApi.remove(a.id); loadArticles() }, 'Article deleted')
  }

  const addCategory = (e) => {
    e.preventDefault()
    run(async () => { await categoryApi.create({ name: newCategory }); setNewCategory(''); loadCategories() }, 'Category added')
  }

  const set = (key) => (e) => setForm({ ...form, [key]: e.target.type === 'checkbox' ? e.target.checked : e.target.value })

  return (
    <section>
      <h1>Admin Panel</h1>
      {msg && <p className="success">{msg}</p>}
      {error && <p className="error">{error}</p>}

      <div className="admin-layout">
        <form className="card form" onSubmit={save}>
          <h2>{editingId ? `Edit article #${editingId}` : 'Publish new article'}</h2>
          <input required placeholder="Headline" value={form.title} onChange={set('title')} />
          <input placeholder="Short summary" value={form.summary} onChange={set('summary')} maxLength={500} />
          <textarea required rows={10} placeholder="Content (leave a blank line between paragraphs)"
                    value={form.content} onChange={set('content')} />
          <input placeholder="Image URL" value={form.imageUrl} onChange={set('imageUrl')} />
          <div className="row">
            <input required placeholder="Author" value={form.author} onChange={set('author')} />
            <select required value={form.categoryId} onChange={set('categoryId')}>
              <option value="">Category</option>
              {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>
          <label className="checkbox">
            <input type="checkbox" checked={form.featured} onChange={set('featured')} /> Featured on home page
          </label>
          <div className="row">
            <button className="btn">{editingId ? 'Update' : 'Publish'}</button>
            {editingId && <button type="button" className="btn secondary"
                                  onClick={() => { setEditingId(null); setForm(emptyArticle) }}>Cancel</button>}
          </div>
        </form>

        <div className="card form">
          <h2>Categories</h2>
          <form className="row" onSubmit={addCategory}>
            <input required placeholder="New category" value={newCategory} onChange={(e) => setNewCategory(e.target.value)} />
            <button className="btn">Add</button>
          </form>
          <ul className="category-list">
            {categories.map((c) => (
              <li key={c.id}>
                {c.name}
                <button className="link-btn danger"
                        onClick={() => run(async () => { await categoryApi.remove(c.id); loadCategories() }, 'Category deleted')}>
                  ✕
                </button>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <h2>All articles {result && <small className="muted">({result.totalElements})</small>}</h2>
      <table>
        <thead><tr><th>Headline</th><th>Category</th><th>Published</th><th>Views</th><th></th></tr></thead>
        <tbody>
          {result?.content.map((a) => (
            <tr key={a.id}>
              <td><Link to={`/articles/${a.id}`}>{a.title}</Link> {a.featured && <span className="tag">Featured</span>}</td>
              <td>{a.category.name}</td>
              <td>{formatDate(a.publishedAt)}</td>
              <td>{a.views}</td>
              <td className="actions">
                <button className="link-btn" onClick={() => edit(a.id)}>Edit</button>
                <button className="link-btn danger" onClick={() => remove(a)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {result && <Pagination page={result.page} totalPages={result.totalPages} onChange={setPage} />}
    </section>
  )
}
