namespace BDEWrapper
{
	/// <summary>
	/// Indicates whether the contents of the volume are accessible from the currently running operating system.
	/// </summary>
	public enum LockedStatus
    {
        /// <summary>
        /// The full contents of the volume are accessible.
        /// </summary>
        Unlocked = 0,
        /// <summary>
        /// All or a portion of the contents of the volume are not accessible.
        /// </summary>
        Locked = 1,
    }
}
