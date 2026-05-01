#!/bin/sh

ls -1 src | grep -G "^Monolith.Modules" | cut -d. -f1-3 | sort -u
