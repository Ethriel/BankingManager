
import { Routes, Route } from 'react-router-dom';
import './App.css';

import { withdrow } from './components/bank-account-manipulations/withdrow';
import { appComponent } from './components/app-component';
import { transfer } from './components/bank-account-manipulations/transfer';
import { deposit } from './components/bank-account-manipulations/deposit';
import { create } from './components/create-bank-account';

function App() {
  return (
    <Routes>
        <Route path='/' Component={appComponent}/>
        <Route path='/transfer' Component={transfer}></Route>
        <Route path='/withdrow' Component={withdrow}></Route>
        <Route path='/deposit' Component={deposit}></Route>
        <Route path='/create' Component={create}></Route>
      </Routes>
  )
}

export default App;