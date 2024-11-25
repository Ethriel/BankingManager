import { ApiMethods } from '../../constants/api-methods'
import { ApiRoutes } from '../../constants/api-routes'
import { useSortableTable } from '../../useSortableTable'
import getErrorsAsString from '../../utilities/get-errors-as-string'
import makeRequestAsync from '../../utilities/make-request-async'
import axios from 'axios'

import { TableBody } from './table-body'
import { TableHead } from './table-head'

const CustomTable = ({
  data,
  columns,
  initialData,
  currencies,
  refreshAccounts: refreshAccounts = null,
}) => {
  const handleRemoveAccount = async (event, id) => {
    const signal = axios.CancelToken.source()

    try {
      await makeRequestAsync(
        ApiRoutes.delete,
        signal.token,
        id,
        ApiMethods.delete
      )
      alert('Account removed')
    } catch (error) {
      const errors = getErrorsAsString(error)
      alert(errors)
    } finally {
      refreshAccounts?.()
    }
  }

  const handleUpdateAccount = async (event, accountsData, accountId) => {
    const signal = axios.CancelToken.source()
    const account = accountsData.filter((c) => c.id === accountId)[0]
    try {
      await makeRequestAsync(
        ApiRoutes.update,
        signal.token,
        account,
        ApiMethods.put
      )
      alert('Account updated')
    } catch (error) {
      const errors = getErrorsAsString(error)
      alert(errors)
    } finally {
      refreshAccounts?.()
    }
  }

  const [tableData, setTableData, handleSorting] = useSortableTable(
    data,
    initialData,
    columns
  )

  return (
    <>
      <h2>Accounts list</h2>
      <table className="table table-striped" aria-labelledby="tableLabel">
        <TableHead columns={columns} handleSorting={handleSorting} />
        <TableBody
          columns={columns}
          currencies={currencies}
          tableData={tableData}
          handleRemove={handleRemoveAccount}
          handleUpdate={handleUpdateAccount}
          setTableData={setTableData}
        />
      </table>
    </>
  )
}

export default CustomTable
