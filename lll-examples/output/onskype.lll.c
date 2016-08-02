#include <stddef.h>
#include <stdio.h>
#include <stdint.h>
struct struc_Date_0;
struct struc_Person_1;

void lll_Main(void);
void ext_Date_Ctor_11(struct struc_Date_0* var_this_12, int8_t var_month_13, int8_t var_day_14, int16_t var_year_15);
void ext_Person_Ctor_16(struct struc_Person_1* var_this_17, char* var_name_18, char* var_familyName_19, struct struc_Date_0* var_birthDay_20);
int32_t ext_Date_AgeYears_21(struct struc_Date_0* var_this_22, struct struc_Date_0* var_other_23);
void ext_Person_print_24(struct struc_Person_1* var_this_25);
void ext_Person_print_26(struct struc_Person_1* var_this_27, int32_t var_times_28);
char* ext_Date_to_string_29(struct struc_Date_0* var_this_30);
int main(int argc, char** argv);

struct struc_Date_0
{
    int8_t Month;
    int8_t Day;
    int16_t Year;
};
struct struc_Person_1
{
    char* Name;
    char* FamilyName;
    int32_t Age;
    struct struc_Date_0* BirthDay;
};

void lll_Main(void)
{
    uint64_t __tmp43 = sizeof(struct struc_Date_0);
    void* __tmp44 = malloc(__tmp43);
    struct struc_Date_0* var___tmp38_39 = __tmp44;
    int32_t __tmp45 = 8;
    int32_t __tmp46 = 22;
    int32_t __tmp47 = 1991;
    ext_Date_Ctor_11(var___tmp38_39, __tmp45, __tmp46, __tmp47);
    struct struc_Date_0* var_birthday_37 = var___tmp38_39;
    uint64_t __tmp48 = sizeof(struct struc_Person_1);
    void* __tmp49 = malloc(__tmp48);
    struct struc_Person_1* var___tmp41_42 = __tmp49;
    char* __tmp50 = "Max";
    char* __tmp51 = "Mickey";
    ext_Person_Ctor_16(var___tmp41_42, __tmp50, __tmp51, var_birthday_37);
    struct struc_Person_1* var_max_40 = var___tmp41_42;
    ext_Person_print_24(var_max_40);
    char* __tmp52 = "printing with times now!\n";
    int32_t __tmp53 = printf(__tmp52);
    ext_Person_print_26(var_max_40, var_max_40->Age);
    char* __tmp54 = "%s\n";
    char* __tmp55 = ext_Date_to_string_29(var_max_40->BirthDay);
    int32_t __tmp56 = printf(__tmp54, __tmp55);
}
void ext_Date_Ctor_11(struct struc_Date_0* var_this_12, int8_t var_month_13, int8_t var_day_14, int16_t var_year_15)
{
    var_this_12->Month = var_month_13;
    var_this_12->Day = var_day_14;
    var_this_12->Year = var_year_15;
}
void ext_Person_Ctor_16(struct struc_Person_1* var_this_17, char* var_name_18, char* var_familyName_19, struct struc_Date_0* var_birthDay_20)
{
    var_this_17->Name = var_name_18;
    var_this_17->FamilyName = var_familyName_19;
    var_this_17->BirthDay = var_birthDay_20;
    uint64_t __tmp57 = sizeof(struct struc_Date_0);
    void* __tmp58 = malloc(__tmp57);
    struct struc_Date_0* var___tmp32_33 = __tmp58;
    int32_t __tmp59 = 7;
    int32_t __tmp60 = 30;
    int32_t __tmp61 = 2016;
    ext_Date_Ctor_11(var___tmp32_33, __tmp59, __tmp60, __tmp61);
    struct struc_Date_0* var_today_31 = var___tmp32_33;
    int32_t __tmp62 = ext_Date_AgeYears_21(var_today_31, var_birthDay_20);
    var_this_17->Age = __tmp62;
    free(var_today_31);
}
int32_t ext_Date_AgeYears_21(struct struc_Date_0* var_this_22, struct struc_Date_0* var_other_23)
{
    int16_t __tmp63 = var_this_22->Year - var_other_23->Year;
    int16_t var_diff_34 = __tmp63;
    {
        int32_t __tmp64 = 0;
        int __tmp65 = var_diff_34 < __tmp64;
        if (__tmp65)
        {
            int32_t __tmp66 = 1;
            int32_t __tmp67 = (-(__tmp66));
            int32_t __tmp68 = var_diff_34 * __tmp67;
            return __tmp68;
        }
    }
    return var_diff_34;
}
void ext_Person_print_24(struct struc_Person_1* var_this_25)
{
    char* __tmp69 = "My name is %s %s and I am %d years old!\n";
    int32_t __tmp70 = printf(__tmp69, var_this_25->Name, var_this_25->FamilyName, var_this_25->Age);
}
void ext_Person_print_26(struct struc_Person_1* var_this_27, int32_t var_times_28)
{
    {
        int32_t __tmp71 = 0;
        int8_t var_i_35 = __tmp71;
L72:
        {
            int __tmp73 = var_i_35 < var_times_28;
            if (__tmp73)
            {
                int32_t __tmp74 = 1;
                int32_t __tmp75 = var_this_27->Age - __tmp74;
                var_this_27->Age = __tmp75;
                ext_Person_print_24(var_this_27);
                int32_t __tmp76 = 1;
                int32_t __tmp77 = var_i_35 + __tmp76;
                var_i_35 = __tmp77;
                goto L72;
            }
        }
    }
}
char* ext_Date_to_string_29(struct struc_Date_0* var_this_30)
{
    int32_t __tmp78 = 11;
    void* __tmp79 = malloc(__tmp78);
    char* var_str_36 = __tmp79;
    char* __tmp80 = "%02d/%02d/%04d";
    int32_t __tmp81 = sprintf(var_str_36, __tmp80, var_this_30->Month, var_this_30->Day, var_this_30->Year);
    return var_str_36;
}
int main(int argc, char** argv)
{
    lll_Main();
    int32_t __tmp82 = 0;
    return __tmp82;
}

