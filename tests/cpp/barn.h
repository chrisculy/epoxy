#include <string>
#include <vector>

#include "barn_state.h"

namespace farm
{
    class barn
    {
    public:
        barn(const char* name);
        barn(const char16_t* name);
        barn(const char32_t* name);

        static std::unique_ptr<barn> create_with_animals(std::vector<animal*> animals);

        void set_state(barn_state state);
        barn_state get_state() const;

        const char* get_name() const;
        const char16_t* get_name_16() const;
        const char32_t* get_name_32() const;

        void add_animal(animal* animal);
        const std::vector<animal*> get_animals() const;

    private:
        // stored as UTF-8
        std::string _name;
        std::vector<animal*> _animals;
        barn_state _state;
    };
}