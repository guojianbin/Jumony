﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Ivony.Html.Parser;
using System.Web.Compilation;
using System.Web.Hosting;

namespace Ivony.Html.Web
{
  public interface IRequestMapper
  {

    MapInfo MapRequest( HttpRequest request );

  }


  public class DefaultRequestMapper : IRequestMapper
  {

    private static readonly string[] allowsExtensions = new[] { ".html", ".htm", ".aspx" };

    public MapInfo MapRequest( HttpRequest request )
    {
      var virtualPath = request.FilePath;

      if ( !allowsExtensions.Contains( VirtualPathUtility.GetExtension( virtualPath ), StringComparer.OrdinalIgnoreCase ) )
        return null;

      if ( !FileExists( virtualPath ) )
        return null;

      var handlerPath = virtualPath + ".ashx";
      if ( !FileExists( handlerPath ) )
        return null;

      var handler = BuildManager.CreateInstanceFromVirtualPath( handlerPath, typeof( JumonyHandler ) ) as JumonyHandler;
      if ( handler == null )
        return null;

      return new MapInfo( virtualPath, handler );
    }


    private static bool FileExists( string virtualPath )
    {
      return HostingEnvironment.VirtualPathProvider.FileExists( virtualPath );
    }

  }



}
