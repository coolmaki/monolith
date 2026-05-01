#!/bin/bash
set -euo pipefail

lib_primitives="Monolith.Lib.Primitives"
lib_domain="Monolith.Lib.Domain"
lib_application="Monolith.Lib.Application"
lib_infrastructure="Monolith.Lib.Infrastructure"
lib_endpoints="Monolith.Lib.Endpoints"

module="Monolith.Modules.$1"
module_domain="$module.Domain"
module_application="$module.Application"
module_infrastructure="$module.Infrastructure"
module_endpoints="$module.Endpoints"

created_projects=()

rollback() {
    echo "Rolling back..." >&2
    for project in "${created_projects[@]}"; do
        if [ -d "$project" ]; then
            rm -rf "$project"
            echo "  Deleted $project" >&2
        fi
    done
    cd ..
}

create_project() {
    local name=$1
    shift
    echo "Creating $name..."
    if ! dotnet new classlib --name "$name" -v q > /dev/null; then
        echo "Failed to create $name" >&2
        rollback
        exit 1
    fi
    created_projects+=("$name")
    rm "$name/Class1.cs"
    if ! dotnet add "$name" reference "$@" > /dev/null; then
        echo "Failed to add references to $name" >&2
        rollback
        exit 1
    fi
}

# Check if module already exists
if ls src/"$module" src/"$module".* 1> /dev/null 2>&1; then
    echo "Error: Module '$module' already exists in src/" >&2
    exit 1
fi

cd src

create_project "$module_domain" \
    "$lib_primitives" \
    "$lib_domain"

create_project "$module_application" \
    "$lib_primitives" \
    "$lib_application" \
    "$module_domain"

create_project "$module_infrastructure" \
    "$lib_primitives" \
    "$lib_infrastructure" \
    "$module_domain" \
    "$module_application"

create_project "$module_endpoints" \
    "$lib_primitives" \
    "$lib_endpoints" \
    "$module_domain" \
    "$module_application"

cd ..

dotnet sln add src/$module.* -s src/Monolith.Modules/$module

echo "DONE"
