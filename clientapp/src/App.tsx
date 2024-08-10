import { Link } from "react-router-dom";
import AppRouting from "./components/AppRouting";
import { useContext } from "react";
import { authContext } from "./contexts";
import "./styles/index.css";

function App() {
  const authCon = useContext(authContext);
  return (
    <div className="app">
      <header className="menu">
        <h2 className="menu-title">Notes</h2>
        <nav>
          <ul className="menu-pages">
            <li className="menu-page">
              <Link to="/">Home</Link>
            </li>
            {authCon.isAuthenticated == false ? (
              <>
                <li className="menu-page">
                  <Link to="/login">Login</Link>
                </li>
                <li className="menu-page">
                  <Link to="/registration">Registration</Link>
                </li>
              </>
            ) : (
              <></>
            )}
          </ul>
        </nav>
      </header>

      <div className="app-body">
        <div>
          <AppRouting />
        </div>
      </div>

      <footer className="app-footer">
        <div>
          <Link to="/policyandprivacy">Policy and Privacy</Link>
        </div>
        <div>Copyright © 2024 Notes</div>
      </footer>
    </div>
  );
}

export default App;
