import { BrowserRouter as Router, Link, Route, Routes } from 'react-router-dom';
import * as React from 'react';
import Projects from './views/Projects';
import './style.css';
import ProjectDetails from './views/ProjectTimeEntries';

export default function App() {
  return (
    <>
      <Router>
        <div className="bg-gray-900 text-white flex items-center h-12 w-full">
          <div className="container mx-auto">
            <Link className="navbar-brand" to="/">
              Home
            </Link>
          </div>
        </div>

        <div className="container mx-auto">
          <Routes>
            <Route path="/" element={<Projects />} />
            <Route path="/project/:projectId" element={<ProjectDetails />} />
          </Routes>
        </div>
      </Router>
    </>
  );
}
