library TestPLMExtension;

{ Important note about DLL memory management: ShareMem must be the
  first unit in your library's USES clause AND your project's (select
  Project-View Source) USES clause if your DLL exports any procedures or
  functions that pass strings as parameters or function results. This
  applies to all strings passed to and from your DLL--even those that
  are nested in records and classes. ShareMem is the interface unit to
  the BORLNDMM.DLL shared memory manager, which must be deployed along
  with your DLL. To avoid using BORLNDMM.DLL, pass string information
  using PChar or ShortString parameters.

  Important note about VCL usage: when this DLL will be implicitly
  loaded and this DLL uses TWicImage / TImageCollection created in
  any unit initialization section, then Vcl.WicImageInit must be
  included into your library's USES clause. }

uses
  System.SysUtils,
  System.Classes,
  TestPLMInterface in 'src\TestPLMInterface.pas',
  TestPLMItems in 'src\TestPLMItems.pas',
  TestPLMParameters in 'src\TestPLMParameters.pas';

{$R *.res}

function SC_PLM_GetInterface: IPLMInterface; stdcall;
begin
  try
    Result := TTestPLMInterface.Create;
  except
    Result := nil;
  end;
end;

procedure SC_PLM_FreeMemory; stdcall;
begin
  // Очистка памяти, если нужно что-то почистить за собой.
  // Нужна для C# (вызвать GC.Collect, иначе CLR dll-ки на C# "держит" интерфейсы переданные в неё из SprutCAM).
end;

exports
  SC_PLM_GetInterface,
  SC_PLM_FreeMemory
  ;

begin
end.
