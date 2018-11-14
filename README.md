# core-blog-vue-demo


# 配合Attribute就可以只拦截相应的方法了。因为拦截器里面是根据# Attribute进行相应判断的！！
# builder.RegisterAssemblyTypes(assembly)
# 　　 .Where(type => typeof(IQCaching).IsAssignableFrom  #           (type) && !type.GetTypeInfo().IsAbstract)     #         .AsImplementedInterfaces()
# 　　     .InstancePerLifetimeScope()
# 　　     .EnableInterfaceInterceptors()
# 　　     .InterceptedBy(typeof(QCachingInterceptor));
#  dynamic proxy consumes a lot of resource, cannot       #       handle high concurrency situation. IL level       #         interception is more effective.