#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
int main(int argc, char** argv);


void lll_Main(void)
{
    {
        int32_t __tmp4 = 0;
        int32_t var_i_3 = __tmp4;
L5:
        {
            int32_t __tmp6 = 300;
            int __tmp7 = var_i_3 <= __tmp6;
            if (__tmp7)
            {
                char* __tmp8 = "%d\n";
                int32_t __tmp9 = printf(__tmp8, var_i_3);
                int32_t __tmp10 = 1;
                int32_t __tmp11 = var_i_3 + __tmp10;
                var_i_3 = __tmp11;
                goto L5;
            }
        }
    }
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp12 = 0;
    return __tmp12;
}

