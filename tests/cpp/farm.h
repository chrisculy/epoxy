#include <memory>
#include <sstream>
#include <string>
#include <vector>

#include "barn.h"
#include "farmer.h"
#include "farm_state.h"

namespace farm
{
    class farm
    {
    public:
        farm(const std::string& name);
        farm(const std::u16string& name);
        farm(const std::u32string& name);
        farm(const std::string& name, std::unique_ptr<farmer> farmer);
        
        void add_barn(std::unique_ptr<barn> barn);
        void set_farmer(std::unique_ptr<farmer> farmer);
        void set_state(farm_state state);
        void set_location(location location);

        const std::string& get_name() const;
        farmer* get_farmer() const;
        farm_state get_state() const;
        location get_location() const;
        const std::vector<std::unique_ptr<barn>>& get_barns() const;

        static int optimal_acre_count = 200

    private:
        // stored as UTF-8
        std::string _name;
        std::unique_ptr<farmer> _farmer;
        std::vector<std::unique_ptr<barn>> _barns;
        farm_state _state;
    };

    std::string to_string(const farm& farm)
    {
        std::ostringstream out;
        out << "Farm '" << farm.get_name() << "' has " << farm.get_barns().size() << " barns and is farmed by " << farm.get_farmer().get_name() << ".";
        return out.str();
    }

    const double c_acres_per_hectare = 2.47105;
}
