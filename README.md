# pshtml
Http hook for powershell scripts

Simply build and Host somewhere. (e.g. IIS)
When the site is accessed, it looks for a $RequestUri.AbsolutePath.html.ps1 file and executes it with the provided request parameters.

Todo: handling of multi value parameters (e.g. ids=1,2,3, untested; may work out of the box)
Todo: Async execution => live http response
Todo: allow for html resources to be accessed (css, js)
