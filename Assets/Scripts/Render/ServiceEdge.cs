using UnityEngine;

public class ServiceEdge: MonoBehaviour {
    public LineRenderer lineRenderer;
    public ParticleSystem normalEmitter;
    public ParticleSystem warningEmitter;
    public ParticleSystem dangerEmitter;

    public void UpdateData(ServiceNode a, ServiceNode b, float normal, float warning, float danger)
    {
        lineRenderer.SetPosition(0, a.transform.position);
        lineRenderer.SetPosition(1, b.transform.position);

        ConfigureEmitter(normalEmitter, normal, b);
        ConfigureEmitter(warningEmitter, warning, b);
        ConfigureEmitter(dangerEmitter, danger, b);
    }

    private Vector3 from()
    {
        return lineRenderer.GetPosition(0);
    }

    private Vector3 to()
    {
        return lineRenderer.GetPosition(1);
    }


    private void ConfigureEmitter(ParticleSystem emitter, float amount, ServiceNode killTrigger)
    {
        var emission = emitter.emission;
        var main = emitter.main;
        var trigger = emitter.trigger;
        emitter.transform.position = from();
        emitter.transform.LookAt(to());
        emitter.transform.Rotate(90, 0, 0);
        emission.rateOverTime = amount;
        trigger.SetCollider(0, killTrigger);
     }
}