using System.Collections.Generic;
using System.Management;

namespace BDEWrapper
{
	/// <summary>
	/// Collection of all Win32_EncryptableVolume instances on the local or remote computer.
	/// </summary>
	public class EncryptedDrives
    {
        //private List<EncryptedDrive> m_oDrives;
        private ManagementObjectSearcher m_oWMISearch;

        /// <summary>
        /// Creates an instance of the class, connecting to WMI on the local computer
        /// </summary>
        public EncryptedDrives()
        {
            connectToNamespace("localhost");
        }

        /// <summary>
        /// Creates an instance of the class, connecting to the specified hostname
        /// </summary>
        /// <param name="Hostname">The remote host you wish to connect to. You must have administrative rights on the specified hostname in order to connect.</param>
        public EncryptedDrives(string Hostname)
        {
            connectToNamespace(Hostname);
        }

        /// <summary>
        /// Creates internal WMI query object pointing to the specified host
        /// </summary>
        /// <param name="HostName">The WMI host to establish a connection on</param>
        private void connectToNamespace(string HostName)
        {
            // setup the management path object
            ManagementPath oWMIPath = new ManagementPath();
            oWMIPath.Server = HostName;
            oWMIPath.NamespacePath = @"root\CIMV2\Security\MicrosoftVolumeEncryption";

            // setup the query object
            ObjectQuery oObjQuery = new ObjectQuery();
            oObjQuery.QueryString = "SELECT DeviceID FROM Win32_EncryptableVolume";

            // setup the connection object as we have to request special authentication
            ConnectionOptions oWMIConnOptions = new ConnectionOptions();
            oWMIConnOptions.Authentication = AuthenticationLevel.PacketPrivacy;

            // bind everything together in a scope
            ManagementScope oWMIScope = new ManagementScope(oWMIPath, oWMIConnOptions);

            // setup the search object
            m_oWMISearch = new ManagementObjectSearcher(oWMIScope, oObjQuery);

            // complete and return
            return;
        }


        /// <summary>
        /// Gets a list of Win32_EncryptableVolume instances on the specified host. 
        /// </summary>
        public List<EncryptedDrive> Drives
        {
            get
            {
                // query for all drive objects
                ManagementObjectCollection oResults = m_oWMISearch.Get();
                List<EncryptedDrive> oDrives = new List<EncryptedDrive>();
                foreach (ManagementObject oResult in oResults)
                {
                    oDrives.Add(new EncryptedDrive((string)oResult["DeviceID"], m_oWMISearch.Scope.Path.Server));
                }
                oResults = null;
                return oDrives;
            }
        }
        
        /// <summary>
        /// Gets the drive. NOT IN USE YET
        /// </summary>
        /// <param name="DriveLetter">The drive letter.</param>
        /// <returns></returns>
        internal EncryptedDrive GetDrive(string DriveLetter)
        {
            throw new System.NotImplementedException();
        }
    }
}
