export default function SmallContainerWithBackground({ children }: { children: React.ReactNode }) {
    return (
        <section className="bg-primary-foreground md:px-4">
            <div className="mx-auto max-w-3xl bg-background p-4">
                <div className="h-52 border-4 border-dotted bg-primary-foreground">
                    {children}
                </div>
            </div>
        </section>
    );
}
