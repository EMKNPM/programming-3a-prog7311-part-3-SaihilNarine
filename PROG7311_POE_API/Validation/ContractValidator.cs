using PROG7311_POE_.Models;

namespace PROG7311_POE_.Validation
{
    public class ContractValidator
    {
        public bool CanCreateRequest(Contract contract, out string error)
        {
            error = "";

            if (contract == null)
            {
                error = "Contract not found";
                return false;
            }

            if (contract.Status != "Active")
            {
                error = "Contract is not active";
                return false;
            }

            if (contract.EndDate <= DateOnly.FromDateTime(DateTime.Now))
            {
                error = "Contract has expired";
                return false;
            }

            return true;
        }
    }
}
