Integration tests that run outside of the test process (the work is delegated to child process and we only check the results). 
These tests run from single-tfm test project, and they themselves can call test projects with multiple TFMs.
