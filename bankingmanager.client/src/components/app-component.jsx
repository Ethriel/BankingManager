import axios from 'axios'
import { useEffect, useState } from 'react';
import '../App.css';
import { ApiRoutes } from '../constants/api-routes';
import CustomTable from './custom-table/custom-table'
import getErrorsAsString from '../utilities/get-errors-as-string'
import makeRequestAsync from '../utilities/make-request-async'
import { useNavigate } from 'react-router-dom';

export const appComponent = () => {
    const navigate = useNavigate()
    const populateBankAccountsData = async () => {
        try {
            setLoading(true)
            const signal = axios.CancelToken.source()
            const response = await makeRequestAsync(
                ApiRoutes.list,
                signal.token
            )

            if (response.status !== 204) {
                const apiResult = response.data
                const originalAccounts = Array.from(apiResult.data)
                setInitialAccounts(originalAccounts)
                setAccounts(originalAccounts)
            }
        } catch (error) {
            //console.log(error);
            const errors = getErrorsAsString(error)
            alert(errors)
        } finally {
            setLoading(false)
        }
    }

    const getCurrenciesList = async () => {
        try {
            setLoading(true)
            const signal = axios.CancelToken.source()
            const response = await makeRequestAsync(
                ApiRoutes.getCurrencies,
                signal.token
            )

            if (response.status !== 204) {
                const currencies = Array.from(response.data)
                setCurrencies(currencies)
            }
        } catch (error) {
            //console.log(error);
            const errors = getErrorsAsString(error)
            alert(errors)
        } finally {
            setLoading(false)
        }
    }
    useEffect(() => {
        populateBankAccountsData();
        getCurrenciesList();
    }, []);

    const [loading, setLoading] = useState(false)
    const [accounts, setAccounts] = useState()
    const [initialAccounts, setInitialAccounts] = useState()
    const [currencies, setCurrencies] = useState()

    const columns = [
        { label: 'Number', accessor: 'number', sortable: true, sortbyOrder: 'desc', editable: false },
        { label: 'Balance', accessor: 'balance', sortable: true, editable: false },
        { label: 'Currency', accessor: 'currency', sortable: true, editable: true },
        { label: 'Deposit', accessor: 'deposit', sortable: false, editable: false },
        { label: 'Withdrow', accessor: 'withdrow', sortable: false, editable: false },
        { label: 'Transfer', accessor: 'transfer', sortable: false, editable: false },
        { label: 'Update', accessor: 'update', sortable: false, editable: false },
        { label: 'Remove', accessor: 'remove', sortable: false, editable: false }
    ]

    return (
        <div>
            <h1>Bank accounts manager</h1>
            <button
                className="button-update"
                onClick={(e) => navigate('/create', { state: { currencies: currencies } })}
                disabled={false}
            >Create</button>
            {loading && (
                <p>
                    <em>Loading...</em>
                </p>
            )}
            {!loading && (accounts === undefined || accounts.length === 0) && (
                <p>
                    <em>No bank accounts to show</em>
                </p>
            )}
            {!loading && accounts !== undefined && accounts.length !== 0 && (
                <CustomTable
                    columns={columns}
                    data={accounts}
                    currencies={currencies}
                    initialData={initialAccounts}
                    refreshAccounts={populateBankAccountsData}
                />
            )}
        </div>
    )
}