#include "farm.h"

namespace farm
{
namespace statistics
{
    class report
    {
    public:
        report(std::string name, farm& farm);

        void run_report() const;
        
        bool get_is_planting() const;

        char get_farm_initial() const;
        char16_t get_farm_initial_16() const;
        char32_t get_farm_initial_32() const;

        char* get_farm_name_array() const;
        char16_t* get_farm_name_array_16() const;
        char32_t* get_farm_name_array_32() const;

        const char* get_farm_name_cstring() const;
        const char16_t* get_farm_name_cstring_16() const;
        const char32_t* get_farm_name_cstring_32() const;

        std::string get_farm_name() const;
        std::u16string get_farm_name_16() const;
        std::u32string get_farm_name_32() const;

        const std::string& get_name() const;
        std::string& get_name_reference() const;
        std::string* get_name_pointer() const;
        
        int8_t get_barn_count_8() const;
        int16_t get_barn_count_16() const;
        int32_t get_barn_count_32() const;
        int64_t get_barn_count_64() const;
        int get_barn_count() const;

    private:
        std::string _name;   
    }
}
}