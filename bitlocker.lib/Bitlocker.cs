using System;
using System.Linq;
using System.Collections.Generic;

namespace BitLocker.Lib
{
    public class Bitlocker
    {
		public class Drive
		{
			BDEWrapper.EncryptedDrive _Drive;
			public Drive(BDEWrapper.EncryptedDrive drv)
			{
				_Drive = drv;
			}

			public string Name => _Drive.DriveLetter;

			public void Mount(string password)
			{
				//_Drive.
				throw new NotImplementedException();
			}

			public bool Locked => _Drive.LockStatus == BDEWrapper.LockedStatus.Locked;
		}

		private static BDEWrapper.EncryptedDrives _bdeDrives;

		private static Lazy<List<Drive>> _drives = new Lazy<List<Bitlocker.Drive>>(
			() =>
			{
				_bdeDrives = new BDEWrapper.EncryptedDrives();

				return _bdeDrives.Drives.Select(d => new Drive(d)).ToList();
			}
		);

		public static IEnumerable<Drive> Drives => _drives.Value;
    }
}
