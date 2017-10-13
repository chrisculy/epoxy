#include <iostream>

#include "animal.h"

namespace farm
{
	class horse : public animal
	{
	public:
		void speak() const override
		{
			std::cout << "Neigh!\n";
		}
	};
}
