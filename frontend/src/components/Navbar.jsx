import { useEffect, useState } from 'react'
import { Link, NavLink } from 'react-router-dom'
import { categoryApi } from '../api'

export default function Navbar() {
  const [categories, setCategories] = useState([])

  useEffect(() => {
    categoryApi.list().then(setCategories).catch(() => setCategories([]))
  }, [])

  return (
    <header className="navbar">
      <div className="navbar-top">
        <Link to="/" className="brand">News<span>Hub</span></Link>
        <span className="date">
          {new Date().toLocaleDateString('en-IN', { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' })}
        </span>
        <NavLink to="/admin" className="admin-link">Admin</NavLink>
      </div>
      <nav className="categories">
        <NavLink to="/" end>Top Stories</NavLink>
        {categories.map((c) => (
          <NavLink key={c.id} to={`/category/${c.id}`}>{c.name}</NavLink>
        ))}
      </nav>
    </header>
  )
}
