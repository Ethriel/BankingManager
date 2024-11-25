import { useLocation, useNavigate } from "react-router-dom";
import makeRequestAsync from "../utilities/make-request-async";
import { ApiRoutes } from "../constants/api-routes";
import axios from "axios";
import { ApiMethods } from "../constants/api-methods";
import { useState } from "react";

export const create = () => {
    const signal = axios.CancelToken.source()
    const navigate = useNavigate()
    const location = useLocation()

    const currencies = location.state.currencies;
    const [bankAccount, setBankAccount] = useState({
        id: 0,
        balance: 0,
        currency: null,
        number: null
    })

    const handleAmmountChange = (e) => {
        setBankAccount({ ...bankAccount, balance: Number(e.target.value) })
    }

    const handleCurrencyChange = (e) => {
        setBankAccount({ ...bankAccount, currency: e.target.value })
    }

    const handleSubmit = async (event) => {
        event.preventDefault()
        try {
            console.log(bankAccount)
            const apiResult = await makeRequestAsync(
                ApiRoutes.create,
                signal.token,
                bankAccount,
                ApiMethods.post
            )
            console.log(apiResult)
            navigate('/')
        } catch (error) {
            console.log(error)
            alert(error)
        }
    }

    const submitDisabled = ((bankAccount.currency === null || bankAccount.currency === '----')) || (bankAccount.balance <= 0)

    return (
        <div>
            <form onSubmit={handleSubmit}
                style={{
                    display: 'flex',
                    flexDirection: 'column'
                }}>
                <label>Ammount</label>
                <input type="number" min={0} onChange={(e) => { handleAmmountChange(e) }} />
                <label>Currency</label>
                <select onChange={(e) => { handleCurrencyChange(e) }}>
                    <option key={'----'}>{'----'}</option>
                    {
                        currencies.map(currency => {
                          return (
                            <option key={currency} value={currency}>{currency}</option>
                          )
                        })
                      }
                </select>
                <button type="submit" disabled={submitDisabled}>Submit</button>
            </form>
        </div>
    )
}