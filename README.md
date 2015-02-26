# ChangeMonitor
Intended for Unity3D; can simplify the detection of changes in properties. However, the ChangeMonitor class does not refer to any part of the Unity3D framework and can be used in another case if you find a use for it.

See ChangeMonitorExample.cs for a Unity3D component utilizing the class. It exposes a set of public members, which can be edited in the Unity3D property editor, and will recognize when any of these figures are changed in the Unity3D editor at runtime.

ChangeMonitor is *not* thread-safe.