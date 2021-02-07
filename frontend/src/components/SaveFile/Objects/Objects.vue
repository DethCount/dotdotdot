<template>
    <div class="row">
        <div class="col-12" ref="objectTree" v-if="tree">
            <Tree v-bind:nodes="tree.entries()"></Tree>
        </div>
    </div>
</template>

<script src="@/components/SaveFile/Objects/Objects.js"></script>

<style lang="sass" scoped src="@/components/SaveFile/Objects/Objects.sass"></style>
