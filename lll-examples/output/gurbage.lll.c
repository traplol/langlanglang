#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
int main(int argc, char** argv);


void lll_Main(void)
{
    char* __tmp4 = "echo Hello world";
    int32_t __tmp5 = system(__tmp4);
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp6 = 0;
    return __tmp6;
}

