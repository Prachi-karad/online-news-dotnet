import { Routes, Route } from 'react-router-dom'
import Navbar from './components/Navbar'
import Home from './pages/Home'
import ArticlePage from './pages/ArticlePage'
import Admin from './pages/Admin'

export default function App() {
  return (
    <>
      <Navbar />
      <main className="container">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/category/:categoryId" element={<Home />} />
          <Route path="/articles/:id" element={<ArticlePage />} />
          <Route path="/admin" element={<Admin />} />
          <Route path="*" element={<p className="empty">Page not found.</p>} />
        </Routes>
      </main>
      <footer className="footer">© {new Date().getFullYear()} NewsHub · Built with ASP.NET Core & React</footer>
    </>
  )
}
