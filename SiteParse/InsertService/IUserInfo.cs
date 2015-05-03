#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
#endregion

namespace InsertService
{
    [ServiceContract]
    public interface IUserInfo
    {
        [OperationContract]
        string InsertUserInfo();
    }

   



}
