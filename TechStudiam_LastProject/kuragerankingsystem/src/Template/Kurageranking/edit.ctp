<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Kurageranking $kurageranking
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Form->postLink(
                __('Delete'),
                ['action' => 'delete', $kurageranking->Id],
                ['confirm' => __('Are you sure you want to delete # {0}?', $kurageranking->Id)]
            )
        ?></li>
        <li><?= $this->Html->link(__('List Kurageranking'), ['action' => 'index']) ?></li>
    </ul>
</nav>
<div class="kurageranking form large-9 medium-8 columns content">
    <?= $this->Form->create($kurageranking) ?>
    <fieldset>
        <legend><?= __('Edit Kurageranking') ?></legend>
        <?php
            echo $this->Form->control('Name');
            echo $this->Form->control('Score');
            echo $this->Form->control('Date');
        ?>
    </fieldset>
    <?= $this->Form->button(__('Submit')) ?>
    <?= $this->Form->end() ?>
</div>
