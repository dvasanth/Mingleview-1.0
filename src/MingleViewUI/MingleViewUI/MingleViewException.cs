using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MingleViewException : Exception
{
  public MingleViewException()
  {
  }
  public MingleViewException(string message): base(message)
  {
  }
  public MingleViewException(string message, Exception inner)
      : base(message, inner)
  {
  }

}

