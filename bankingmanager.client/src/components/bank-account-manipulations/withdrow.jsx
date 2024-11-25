import { useState } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import makeRequestAsync from "../../utilities/make-request-async";
import { ApiRoutes } from "../../constants/api-routes";
import axios from "axios";
import { ApiMethods } from "../../constants/api-methods";

export const withdrow = () => {
    const signal = axios.CancelToken.source()
    const navigate = useNavigate()
    const location = useLocation()
    const account = location.state.account;
    const [bankAccountAction, setBankAccountAction] = useState({
        accountNumber: account.number,
        ammount: 0,
        currency: account.currency
    })

    const handleAmmountChange = (e) => {
        setBankAccountAction({ ...bankAccountAction, ammount: e.target.value })
    }

    const handleSubmit = async (event) => {
        event.preventDefault()
        console.log(bankAccountAction)
        try {
            const apiResult = await makeRequestAsync(
                ApiRoutes.withdrow,
                signal.token,
                bankAccountAction,
                ApiMethods.post
            )
            console.log(apiResult)
            navigate('/')
        } catch (error) {
            alert(error)
        }
    }
    return (
        <div>
            <form onSubmit={handleSubmit}
                style={{
                    display: 'flex',
                    flexDirection: 'column'
                }}>
                <label>Ammount</label>
                <input type="number" min={0} max={account.balance} onChange={handleAmmountChange} />
                <button type="submit">Submit</button>
            </form>
        </div>
    )
}