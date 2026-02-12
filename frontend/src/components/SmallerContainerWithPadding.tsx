export default function SmallerContainerWithPadding({ children }: { children: React.ReactNode }) {
    return (
        <section className="mx-auto w-full max-w-3xl px-4">
            <div className="h-52 border-4 border-dotted bg-primary-foreground">
                {children}
            </div>
        </section>
    );
}
