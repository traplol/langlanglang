#include <stddef.h>
#include <stdio.h>
#include <stdint.h>

void lll_Main(void);
int32_t func_F_3(void);
int32_t func_T_4(void);
int main(int argc, char** argv);


void lll_Main(void)
{
    {
        int __tmp5 = 0;
        {
            int __tmp6 = 0;
            {
                int __tmp7 = 0;
                {
                    int32_t __tmp8 = func_F_3();
                    if (__tmp8)
                    {
                        int32_t __tmp9 = 1;
                        __tmp7 = __tmp9;
                        {
                            int32_t __tmp10 = func_F_3();
                            if (__tmp10)
                            {
                                int32_t __tmp11 = 1;
                                __tmp7 = __tmp11;
                            }
                            else
                            {
                                int32_t __tmp12 = 0;
                                __tmp7 = __tmp12;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp13 = 0;
                        __tmp7 = __tmp13;
                    }
                }
                if (__tmp7)
                {
                    int32_t __tmp14 = 1;
                    __tmp6 = __tmp14;
                    {
                        int32_t __tmp15 = func_F_3();
                        if (__tmp15)
                        {
                            int32_t __tmp16 = 1;
                            __tmp6 = __tmp16;
                        }
                        else
                        {
                            int32_t __tmp17 = 0;
                            __tmp6 = __tmp17;
                        }
                    }
                }
                else
                {
                    int32_t __tmp18 = 0;
                    __tmp6 = __tmp18;
                }
            }
            if (__tmp6)
            {
                int32_t __tmp19 = 1;
                __tmp5 = __tmp19;
            }
            else
            {
                {
                    int32_t __tmp20 = func_T_4();
                    if (__tmp20)
                    {
                        int32_t __tmp21 = 1;
                        __tmp5 = __tmp21;
                    }
                }
            }
        }
        if (__tmp5)
        {
            char* __tmp22 = "-> true\n";
            int32_t __tmp23 = printf(__tmp22);
        }
        else
        {
            char* __tmp24 = "-> false\n";
            int32_t __tmp25 = printf(__tmp24);
        }
    }
    {
        int __tmp26 = 0;
        {
            int __tmp27 = 0;
            {
                int __tmp28 = 0;
                {
                    int32_t __tmp29 = func_F_3();
                    if (__tmp29)
                    {
                        int32_t __tmp30 = 1;
                        __tmp28 = __tmp30;
                        {
                            int32_t __tmp31 = func_F_3();
                            if (__tmp31)
                            {
                                int32_t __tmp32 = 1;
                                __tmp28 = __tmp32;
                            }
                            else
                            {
                                int32_t __tmp33 = 0;
                                __tmp28 = __tmp33;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp34 = 0;
                        __tmp28 = __tmp34;
                    }
                }
                if (__tmp28)
                {
                    int32_t __tmp35 = 1;
                    __tmp27 = __tmp35;
                    {
                        int32_t __tmp36 = func_T_4();
                        if (__tmp36)
                        {
                            int32_t __tmp37 = 1;
                            __tmp27 = __tmp37;
                        }
                        else
                        {
                            int32_t __tmp38 = 0;
                            __tmp27 = __tmp38;
                        }
                    }
                }
                else
                {
                    int32_t __tmp39 = 0;
                    __tmp27 = __tmp39;
                }
            }
            if (__tmp27)
            {
                int32_t __tmp40 = 1;
                __tmp26 = __tmp40;
            }
            else
            {
                {
                    int32_t __tmp41 = func_T_4();
                    if (__tmp41)
                    {
                        int32_t __tmp42 = 1;
                        __tmp26 = __tmp42;
                    }
                }
            }
        }
        if (__tmp26)
        {
            char* __tmp43 = "-> true\n";
            int32_t __tmp44 = printf(__tmp43);
        }
        else
        {
            char* __tmp45 = "-> false\n";
            int32_t __tmp46 = printf(__tmp45);
        }
    }
    {
        int __tmp47 = 0;
        {
            int __tmp48 = 0;
            {
                int __tmp49 = 0;
                {
                    int32_t __tmp50 = func_F_3();
                    if (__tmp50)
                    {
                        int32_t __tmp51 = 1;
                        __tmp49 = __tmp51;
                        {
                            int32_t __tmp52 = func_T_4();
                            if (__tmp52)
                            {
                                int32_t __tmp53 = 1;
                                __tmp49 = __tmp53;
                            }
                            else
                            {
                                int32_t __tmp54 = 0;
                                __tmp49 = __tmp54;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp55 = 0;
                        __tmp49 = __tmp55;
                    }
                }
                if (__tmp49)
                {
                    int32_t __tmp56 = 1;
                    __tmp48 = __tmp56;
                    {
                        int32_t __tmp57 = func_F_3();
                        if (__tmp57)
                        {
                            int32_t __tmp58 = 1;
                            __tmp48 = __tmp58;
                        }
                        else
                        {
                            int32_t __tmp59 = 0;
                            __tmp48 = __tmp59;
                        }
                    }
                }
                else
                {
                    int32_t __tmp60 = 0;
                    __tmp48 = __tmp60;
                }
            }
            if (__tmp48)
            {
                int32_t __tmp61 = 1;
                __tmp47 = __tmp61;
            }
            else
            {
                {
                    int32_t __tmp62 = func_T_4();
                    if (__tmp62)
                    {
                        int32_t __tmp63 = 1;
                        __tmp47 = __tmp63;
                    }
                }
            }
        }
        if (__tmp47)
        {
            char* __tmp64 = "-> true\n";
            int32_t __tmp65 = printf(__tmp64);
        }
        else
        {
            char* __tmp66 = "-> false\n";
            int32_t __tmp67 = printf(__tmp66);
        }
    }
    {
        int __tmp68 = 0;
        {
            int __tmp69 = 0;
            {
                int __tmp70 = 0;
                {
                    int32_t __tmp71 = func_T_4();
                    if (__tmp71)
                    {
                        int32_t __tmp72 = 1;
                        __tmp70 = __tmp72;
                        {
                            int32_t __tmp73 = func_F_3();
                            if (__tmp73)
                            {
                                int32_t __tmp74 = 1;
                                __tmp70 = __tmp74;
                            }
                            else
                            {
                                int32_t __tmp75 = 0;
                                __tmp70 = __tmp75;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp76 = 0;
                        __tmp70 = __tmp76;
                    }
                }
                if (__tmp70)
                {
                    int32_t __tmp77 = 1;
                    __tmp69 = __tmp77;
                    {
                        int32_t __tmp78 = func_F_3();
                        if (__tmp78)
                        {
                            int32_t __tmp79 = 1;
                            __tmp69 = __tmp79;
                        }
                        else
                        {
                            int32_t __tmp80 = 0;
                            __tmp69 = __tmp80;
                        }
                    }
                }
                else
                {
                    int32_t __tmp81 = 0;
                    __tmp69 = __tmp81;
                }
            }
            if (__tmp69)
            {
                int32_t __tmp82 = 1;
                __tmp68 = __tmp82;
            }
            else
            {
                {
                    int32_t __tmp83 = func_T_4();
                    if (__tmp83)
                    {
                        int32_t __tmp84 = 1;
                        __tmp68 = __tmp84;
                    }
                }
            }
        }
        if (__tmp68)
        {
            char* __tmp85 = "-> true\n";
            int32_t __tmp86 = printf(__tmp85);
        }
        else
        {
            char* __tmp87 = "-> false\n";
            int32_t __tmp88 = printf(__tmp87);
        }
    }
    {
        int __tmp89 = 0;
        {
            int __tmp90 = 0;
            {
                int __tmp91 = 0;
                {
                    int32_t __tmp92 = func_T_4();
                    if (__tmp92)
                    {
                        int32_t __tmp93 = 1;
                        __tmp91 = __tmp93;
                        {
                            int32_t __tmp94 = func_F_3();
                            if (__tmp94)
                            {
                                int32_t __tmp95 = 1;
                                __tmp91 = __tmp95;
                            }
                            else
                            {
                                int32_t __tmp96 = 0;
                                __tmp91 = __tmp96;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp97 = 0;
                        __tmp91 = __tmp97;
                    }
                }
                if (__tmp91)
                {
                    int32_t __tmp98 = 1;
                    __tmp90 = __tmp98;
                    {
                        int32_t __tmp99 = func_F_3();
                        if (__tmp99)
                        {
                            int32_t __tmp100 = 1;
                            __tmp90 = __tmp100;
                        }
                        else
                        {
                            int32_t __tmp101 = 0;
                            __tmp90 = __tmp101;
                        }
                    }
                }
                else
                {
                    int32_t __tmp102 = 0;
                    __tmp90 = __tmp102;
                }
            }
            if (__tmp90)
            {
                int32_t __tmp103 = 1;
                __tmp89 = __tmp103;
                {
                    int32_t __tmp104 = func_T_4();
                    if (__tmp104)
                    {
                        int32_t __tmp105 = 1;
                        __tmp89 = __tmp105;
                    }
                    else
                    {
                        int32_t __tmp106 = 0;
                        __tmp89 = __tmp106;
                    }
                }
            }
            else
            {
                int32_t __tmp107 = 0;
                __tmp89 = __tmp107;
            }
        }
        if (__tmp89)
        {
            char* __tmp108 = "-> true\n";
            int32_t __tmp109 = printf(__tmp108);
        }
        else
        {
            char* __tmp110 = "-> false\n";
            int32_t __tmp111 = printf(__tmp110);
        }
    }
    {
        int __tmp112 = 0;
        {
            int __tmp113 = 0;
            {
                int __tmp114 = 0;
                {
                    int32_t __tmp115 = func_T_4();
                    if (__tmp115)
                    {
                        int32_t __tmp116 = 1;
                        __tmp114 = __tmp116;
                        {
                            int32_t __tmp117 = func_F_3();
                            if (__tmp117)
                            {
                                int32_t __tmp118 = 1;
                                __tmp114 = __tmp118;
                            }
                            else
                            {
                                int32_t __tmp119 = 0;
                                __tmp114 = __tmp119;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp120 = 0;
                        __tmp114 = __tmp120;
                    }
                }
                if (__tmp114)
                {
                    int32_t __tmp121 = 1;
                    __tmp113 = __tmp121;
                    {
                        int32_t __tmp122 = func_F_3();
                        if (__tmp122)
                        {
                            int32_t __tmp123 = 1;
                            __tmp113 = __tmp123;
                        }
                        else
                        {
                            int32_t __tmp124 = 0;
                            __tmp113 = __tmp124;
                        }
                    }
                }
                else
                {
                    int32_t __tmp125 = 0;
                    __tmp113 = __tmp125;
                }
            }
            if (__tmp113)
            {
                int32_t __tmp126 = 1;
                __tmp112 = __tmp126;
                {
                    int32_t __tmp127 = func_T_4();
                    if (__tmp127)
                    {
                        int32_t __tmp128 = 1;
                        __tmp112 = __tmp128;
                    }
                    else
                    {
                        int32_t __tmp129 = 0;
                        __tmp112 = __tmp129;
                    }
                }
            }
            else
            {
                int32_t __tmp130 = 0;
                __tmp112 = __tmp130;
            }
        }
        if (__tmp112)
        {
            char* __tmp131 = "-> true\n";
            int32_t __tmp132 = printf(__tmp131);
        }
        else
        {
            char* __tmp133 = "-> false\n";
            int32_t __tmp134 = printf(__tmp133);
        }
    }
    {
        int __tmp135 = 0;
        {
            int __tmp136 = 0;
            {
                int __tmp137 = 0;
                {
                    int32_t __tmp138 = func_T_4();
                    if (__tmp138)
                    {
                        int32_t __tmp139 = 1;
                        __tmp137 = __tmp139;
                        {
                            int32_t __tmp140 = func_F_3();
                            if (__tmp140)
                            {
                                int32_t __tmp141 = 1;
                                __tmp137 = __tmp141;
                            }
                            else
                            {
                                int32_t __tmp142 = 0;
                                __tmp137 = __tmp142;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp143 = 0;
                        __tmp137 = __tmp143;
                    }
                }
                if (__tmp137)
                {
                    int32_t __tmp144 = 1;
                    __tmp136 = __tmp144;
                    {
                        int32_t __tmp145 = func_F_3();
                        if (__tmp145)
                        {
                            int32_t __tmp146 = 1;
                            __tmp136 = __tmp146;
                        }
                        else
                        {
                            int32_t __tmp147 = 0;
                            __tmp136 = __tmp147;
                        }
                    }
                }
                else
                {
                    int32_t __tmp148 = 0;
                    __tmp136 = __tmp148;
                }
            }
            if (__tmp136)
            {
                int32_t __tmp149 = 1;
                __tmp135 = __tmp149;
                {
                    int32_t __tmp150 = func_T_4();
                    if (__tmp150)
                    {
                        int32_t __tmp151 = 1;
                        __tmp135 = __tmp151;
                    }
                    else
                    {
                        int32_t __tmp152 = 0;
                        __tmp135 = __tmp152;
                    }
                }
            }
            else
            {
                int32_t __tmp153 = 0;
                __tmp135 = __tmp153;
            }
        }
        if (__tmp135)
        {
            char* __tmp154 = "-> true\n";
            int32_t __tmp155 = printf(__tmp154);
        }
        else
        {
            char* __tmp156 = "-> false\n";
            int32_t __tmp157 = printf(__tmp156);
        }
    }
    {
        int __tmp158 = 0;
        {
            int __tmp159 = 0;
            {
                int __tmp160 = 0;
                {
                    int32_t __tmp161 = func_T_4();
                    if (__tmp161)
                    {
                        int32_t __tmp162 = 1;
                        __tmp160 = __tmp162;
                        {
                            int32_t __tmp163 = func_T_4();
                            if (__tmp163)
                            {
                                int32_t __tmp164 = 1;
                                __tmp160 = __tmp164;
                            }
                            else
                            {
                                int32_t __tmp165 = 0;
                                __tmp160 = __tmp165;
                            }
                        }
                    }
                    else
                    {
                        int32_t __tmp166 = 0;
                        __tmp160 = __tmp166;
                    }
                }
                if (__tmp160)
                {
                    int32_t __tmp167 = 1;
                    __tmp159 = __tmp167;
                    {
                        int32_t __tmp168 = func_T_4();
                        if (__tmp168)
                        {
                            int32_t __tmp169 = 1;
                            __tmp159 = __tmp169;
                        }
                        else
                        {
                            int32_t __tmp170 = 0;
                            __tmp159 = __tmp170;
                        }
                    }
                }
                else
                {
                    int32_t __tmp171 = 0;
                    __tmp159 = __tmp171;
                }
            }
            if (__tmp159)
            {
                int32_t __tmp172 = 1;
                __tmp158 = __tmp172;
                {
                    int32_t __tmp173 = func_T_4();
                    if (__tmp173)
                    {
                        int32_t __tmp174 = 1;
                        __tmp158 = __tmp174;
                    }
                    else
                    {
                        int32_t __tmp175 = 0;
                        __tmp158 = __tmp175;
                    }
                }
            }
            else
            {
                int32_t __tmp176 = 0;
                __tmp158 = __tmp176;
            }
        }
        if (__tmp158)
        {
            char* __tmp177 = "-> true\n";
            int32_t __tmp178 = printf(__tmp177);
        }
        else
        {
            char* __tmp179 = "-> false\n";
            int32_t __tmp180 = printf(__tmp179);
        }
    }
    {
        int __tmp181 = 0;
        {
            int __tmp182 = 0;
            {
                int32_t __tmp183 = func_T_4();
                if (__tmp183)
                {
                    int32_t __tmp184 = 1;
                    __tmp182 = __tmp184;
                    {
                        int32_t __tmp185 = func_F_3();
                        if (__tmp185)
                        {
                            int32_t __tmp186 = 1;
                            __tmp182 = __tmp186;
                        }
                        else
                        {
                            int32_t __tmp187 = 0;
                            __tmp182 = __tmp187;
                        }
                    }
                }
                else
                {
                    int32_t __tmp188 = 0;
                    __tmp182 = __tmp188;
                }
            }
            if (__tmp182)
            {
                int32_t __tmp189 = 1;
                __tmp181 = __tmp189;
            }
            else
            {
                {
                    int __tmp190 = 0;
                    {
                        int32_t __tmp191 = func_F_3();
                        if (__tmp191)
                        {
                            int32_t __tmp192 = 1;
                            __tmp190 = __tmp192;
                            {
                                int32_t __tmp193 = func_T_4();
                                if (__tmp193)
                                {
                                    int32_t __tmp194 = 1;
                                    __tmp190 = __tmp194;
                                }
                                else
                                {
                                    int32_t __tmp195 = 0;
                                    __tmp190 = __tmp195;
                                }
                            }
                        }
                        else
                        {
                            int32_t __tmp196 = 0;
                            __tmp190 = __tmp196;
                        }
                    }
                    if (__tmp190)
                    {
                        int32_t __tmp197 = 1;
                        __tmp181 = __tmp197;
                    }
                }
            }
        }
        if (__tmp181)
        {
            char* __tmp198 = "-> true\n";
            int32_t __tmp199 = printf(__tmp198);
        }
        else
        {
            char* __tmp200 = "-> false\n";
            int32_t __tmp201 = printf(__tmp200);
        }
    }
    {
        int __tmp202 = 0;
        {
            int32_t __tmp203 = func_F_3();
            if (__tmp203)
            {
                int32_t __tmp204 = 1;
                __tmp202 = __tmp204;
            }
            else
            {
                {
                    int32_t __tmp205 = func_T_4();
                    if (__tmp205)
                    {
                        int32_t __tmp206 = 1;
                        __tmp202 = __tmp206;
                    }
                }
            }
        }
        if (__tmp202)
        {
            char* __tmp207 = "-> true\n";
            int32_t __tmp208 = printf(__tmp207);
        }
        else
        {
            char* __tmp209 = "-> false\n";
            int32_t __tmp210 = printf(__tmp209);
        }
    }
}
int32_t func_F_3(void)
{
    char* __tmp211 = "F ";
    int32_t __tmp212 = printf(__tmp211);
    int32_t __tmp213 = 0;
    return __tmp213;
}
int32_t func_T_4(void)
{
    char* __tmp214 = "T ";
    int32_t __tmp215 = printf(__tmp214);
    int32_t __tmp216 = 1;
    return __tmp216;
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp217 = 0;
    return __tmp217;
}

