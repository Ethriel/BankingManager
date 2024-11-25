import { useState } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import makeRequestAsync from "../../utilities/make-request-async"
import { ApiRoutes } from "../../constants/api-routes"
import axios from "axios"
import { ApiMethods } from "../../constants/api-methods"

export const transfer = () => {
    const location = useLocation()
    const navigate = useNavigate()

    const accountFrom = location.state.account

    const availableAccounts = location.state.allAccounts.tableData.filter(acc => {
        if (acc.number !== accountFrom.number && acc.currency === accountFrom.currency)
            return acc
    })

    const [transferAction, setTransferAction] = useState({
        fromAccountNumber: accountFrom.number,
        toAccountNumber: null,
        currency: accountFrom.currency,
        ammount: 0
    })

    const handleAmmountChange = (e) => {
        setTransferAction({ ...transferAction, ammount: e.target.value })
    }

    const handleToAccountChange = (e) => {
        setTransferAction({ ...transferAction, toAccountNumber: e.target.value })
    }

    const handleSubmit = async (event) => {
        event.preventDefault()
        try {
            const signal = axios.CancelToken.source()
            const apiResult = await makeRequestAsync(
                ApiRoutes.transfer,
                signal.token,
                transferAction,
                ApiMethods.post
            )
            console.log(apiResult)
            navigate('/')
        } catch (error) {
            console.log(error)
        }
    }

    const submitDisabled = ((transferAction.toAccountNumber === null || transferAction.toAccountNumber === '----')) || (transferAction.ammount <= 0)

    return (
        <div>
            <form onSubmit={handleSubmit}
                style={{
                    display: 'flex',
                    flexDirection: 'column'
                }}>
                <label>Ammount</label>
                <input type="number" min={0} max={accountFrom.balance} onChange={(e) => { handleAmmountChange(e) }} />
                <label>To account</label>
                <select onChange={(e) => { handleToAccountChange(e) }}>
                    <option key={'----'}>{'----'}</option>
                    {
                        availableAccounts.map(acc => {
                            return <option value={acc.number} key={acc.number} >{acc.number}</option>
                        })
                    }
                </select>
                <button type="submit" disabled={submitDisabled}>Submit</button>
            </form>
        </div>
    )
}