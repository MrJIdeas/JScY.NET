using Python.Runtime;

//ANNNI_Classic_1D test = new ANNNI_Classic_1D(-1, 0.3, 0, 1, 10, EParticleType.ZBoson, ELatticeBoundary.Periodic, 1);
//test.Start();

//while (test.n < 1000) ;
//test.Stop();
PythonEngine.Initialize();
using (Py.GIL())
{
    PythonEngine.Exec("a=1");
}