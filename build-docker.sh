#!/bin/bash
docker build --tag=simp_bot .
docker run --rm --name=simp_bot -it simp_bot