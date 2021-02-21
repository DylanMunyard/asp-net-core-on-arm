# asp-net-core-on-arm
Getting ASP.NET Core to run on a Raspberry Pi

## Working
.NET Core supports building for multiple architectures via the `--runtime / r` flag
on dotnet publish and dotnet restore. The ASP.NET Core runtime image
has multiple different tags for target architectures.
[Dockerfile](Dockerfile) builds ASP.NET Core for arm32v7.

`docker build . -t dylanmunyard/arm-hello-world:1.2` \
`docker push dylanmunyard/arm-hello-world:1.2`

## Using buildx - Rabbit hole - Multi-architecture Docker build support
> The reason this doesn't work is that .NET Core doesn't support running 
> within QEMU which provides Docker with support for building images for multiple platforms. 
> The solution is to use dotnet core's runtime identifier to publish 
> for the target architecture, and to use the arm32v7 tag of the ASP.NET Core
> runtime image. 
> [source](https://github.com/dotnet/dotnet-docker/issues/1291)
> 
> However these steps do allow you to run remote Docker builds on the 
> Raspberry Pi itself. 

https://www.docker.com/blog/getting-started-with-docker-for-arm-on-linux/ is a pretty good overview
on how to build Docker images for multiple platforms. 

### Steps
- Download docker-buildx https://github.com/docker/buildx/releases/tag/v0.2.0
- Copy to ~/.docker/cli-plugins
- Make it executable `chmod +x ~/.docker/cli-plugins/docker-buildx`
- Confirm it was installed: `docker buildx --help`:
```angular2html
Usage:	docker buildx COMMAND

Build with BuildKit

Management Commands:
  imagetools  Commands to work on images in registry

Commands:
  bake        Build from a file
  build       Start a build
  create      Create a new builder instance
  inspect     Inspect current builder instance
  ls          List builder instances
  rm          Remove a builder instance
  stop        Stop builder instance
  use         Set the current builder instance
  version     Show buildx version information 

Run 'docker buildx COMMAND --help' for more information on a command.
```

- Install multi-architecture Dockerfile support:
`docker run --rm --privileged multiarch/qemu-user-static --reset -p yes`\
- Create a docker build instance: 
  ```
  docker buildx create --name mybuilder
  docker buildx use mybuilder
  docker buildx inspect --bootstrap
  ```
 - The output of inspect should list linux/arm/v7 in the platforms:
```
Name:   builder
Driver: docker-container

Nodes:
Name:      builder0
Endpoint:  unix:///var/run/docker.sock
Status:    running
Platforms: linux/amd64, linux/arm64, linux/riscv64, linux/ppc64le, linux/s390x, linux/386, linux/arm/v7, linux/arm/v6
```

## Build the image for arm
`docker buildx build --platform linux/arm -t dylanmunyard/arm-hello-world:1.1 . --push`
